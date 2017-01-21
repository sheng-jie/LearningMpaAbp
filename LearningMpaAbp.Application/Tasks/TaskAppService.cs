using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Dynamic;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using Abp.Domain.Repositories;
using Abp.Extensions;
using Abp.Linq.Extensions;
using Abp.Net.Mail.Smtp;
using Abp.Notifications;
using Abp.Timing;
using AutoMapper;
using LearningMpaAbp.Tasks.Dtos;
using LearningMpaAbp.Users;

namespace LearningMpaAbp.Tasks
{
    /// <summary>
    ///     Implements <see cref="ITaskAppService" /> to perform task related application functionality.
    ///     Inherits from <see cref="ApplicationService" />.
    ///     <see cref="ApplicationService" /> contains some basic functionality common for application services (such as
    ///     logging and localization).
    /// </summary>
    public class TaskAppService : LearningMpaAbpAppServiceBase, ITaskAppService
    {
        private readonly INotificationPublisher _notificationPublisher;
        private readonly ISmtpEmailSenderConfiguration _smtpEmialSenderConfig;
        //These members set in constructor using constructor injection.

        private readonly IRepository<Task> _taskRepository;
        private readonly IRepository<User, long> _userRepository;

        /// <summary>
        ///     In constructor, we can get needed classes/interfaces.
        ///     They are sent here by dependency injection system automatically.
        /// </summary>
        public TaskAppService(IRepository<Task> taskRepository, IRepository<User, long> userRepository,
            ISmtpEmailSenderConfiguration smtpEmialSenderConfigtion, INotificationPublisher notificationPublisher)
        {
            _taskRepository = taskRepository;
            _userRepository = userRepository;
            _smtpEmialSenderConfig = smtpEmialSenderConfigtion;
            _notificationPublisher = notificationPublisher;
        }

        public IList<TaskDto> GetAllTasks()
        {
            var tasks = _taskRepository.GetAll().OrderByDescending(t => t.CreationTime).ToList();
            return Mapper.Map<IList<TaskDto>>(tasks);
        }

        public GetTasksOutput GetTasks(GetTasksInput input)
        {
            var query = _taskRepository.GetAll().Include(t => t.AssignedPerson)
                .WhereIf(input.State.HasValue, t => t.State == input.State.Value)
                .WhereIf(!input.Filter.IsNullOrEmpty(), t => t.Title.Contains(input.Filter))
                .WhereIf(input.AssignedPersonId.HasValue, t => t.AssignedPersonId == input.AssignedPersonId.Value);

            //排序
            if (!string.IsNullOrEmpty(input.Sorting))
                query = query.OrderBy(input.Sorting);
            else
                query = query.OrderByDescending(t => t.CreationTime);
            
            var taskList = query.ToList();

            //Used AutoMapper to automatically convert List<Task> to List<TaskDto>.
            return new GetTasksOutput
            {
                Tasks = Mapper.Map<List<TaskDto>>(taskList)
            };
        }


        public PagedResultDto<TaskDto> GetPagedTasks(GetTasksInput input)
        {
            //初步过滤
            var query = _taskRepository.GetAll().Include(t => t.AssignedPerson)
                .WhereIf(input.State.HasValue, t => t.State == input.State.Value)
                .WhereIf(!input.Filter.IsNullOrEmpty(), t => t.Title.Contains(input.Filter))
                .WhereIf(input.AssignedPersonId.HasValue, t => t.AssignedPersonId == input.AssignedPersonId.Value);

            //排序
            query = !string.IsNullOrEmpty(input.Sorting) ? query.OrderBy(input.Sorting) : query.OrderByDescending(t => t.CreationTime);

            //获取总数
            var tasksCount = query.Count();
            //默认的分页方式
            //var taskList = query.Skip(input.SkipCount).Take(input.MaxResultCount).ToList();

            //ABP提供了扩展方法PageBy分页方式
            var taskList = query.PageBy(input).ToList();

            return new PagedResultDto<TaskDto>(tasksCount,taskList.MapTo<List<TaskDto>>());
        }

        public async Task<TaskDto> GetTaskByIdAsync(int taskId)
        {
            //Called specific GetAllWithPeople method of task repository.
            var task = await _taskRepository.GetAsync(taskId);

            //Used AutoMapper to automatically convert List<Task> to List<TaskDto>.
            return task.MapTo<TaskDto>();
        }

        public TaskDto GetTaskById(int taskId)
        {
            var task = _taskRepository.Get(taskId);

            return task.MapTo<TaskDto>();
        }

        public void UpdateTask(UpdateTaskInput input)
        {
            //We can use Logger, it's defined in ApplicationService base class.
            Logger.Info("Updating a task for input: " + input);
            var updateTask = Mapper.Map<Task>(input);
            _taskRepository.Update(updateTask);

            //Retrieving a task entity with given id using standard Get method of repositories.
            //var task = _taskRepository.Get(input.Id);

            //Updating changed properties of the retrieved task entity.

            //if (input.State.HasValue)
            //{
            //    updateTask.State = input.State.Value;
            //}

            //if (input.AssignedPersonId.HasValue)
            //{
            //    updateTask.AssignedPerson = _userRepository.Load(input.AssignedPersonId.Value);
            //}
            //We even do not call Update method of the repository.
            //Because an application service method is a 'unit of work' scope as default.
            //ABP automatically saves all changes when a 'unit of work' scope ends (without any exception).
        }

        public int CreateTask(CreateTaskInput input)
        {
            //We can use Logger, it's defined in ApplicationService class.
            Logger.Info("Creating a task for input: " + input);

            var task = Mapper.Map<Task>(input);

            task.CreationTime = Clock.Now;

            if (input.AssignedPersonId.HasValue)
            {
                task.AssignedPerson = _userRepository.Load(input.AssignedPersonId.Value);
                var message = "You hava been assigned one task into your todo list.";

                //TODO:需要重新配置QQ邮箱密码
                //SmtpEmailSender emailSender = new SmtpEmailSender(_smtpEmialSenderConfig);
                //emailSender.Send("ysjshengjie@qq.com", task.AssignedPerson.EmailAddress, "New Todo item", message);

                _notificationPublisher.Publish("NewTask", new MessageNotificationData(message), null,
                    NotificationSeverity.Info, new[] {task.AssignedPerson.ToUserIdentifier()});
            }

            //Saving entity with standard Insert method of repositories.
            return _taskRepository.InsertAndGetId(task);
        }

        public void DeleteTask(int taskId)
        {
            var task = _taskRepository.Get(taskId);
            if (task != null)
                _taskRepository.Delete(task);
        }
    }
}