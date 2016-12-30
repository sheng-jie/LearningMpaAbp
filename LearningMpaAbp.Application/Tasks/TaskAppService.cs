using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Dynamic;
using System.Net.Configuration;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using Abp.Domain.Repositories;
using Abp.Linq.Extensions;
using Abp.Timing;
using AutoMapper;
using LearningMpaAbp.Tasks.Dtos;
using LearningMpaAbp.Users;
using Abp.Net.Mail.Smtp;
using System.Net.Mail;
using Abp.Notifications;

namespace LearningMpaAbp.Tasks
{
    /// <summary>
    /// Implements <see cref="ITaskAppService"/> to perform task related application functionality.
    /// 
    /// Inherits from <see cref="ApplicationService"/>.
    /// <see cref="ApplicationService"/> contains some basic functionality common for application services (such as logging and localization).
    /// </summary>
    public class TaskAppService : LearningMpaAbpAppServiceBase, ITaskAppService
    {
        //These members set in constructor using constructor injection.

        private readonly IRepository<Task> _taskRepository;
        private readonly IRepository<User, long> _userRepository;
        private readonly ISmtpEmailSenderConfiguration _smtpEmialSenderConfig;
        private readonly INotificationPublisher _notificationPublisher;

        /// <summary>
        ///In constructor, we can get needed classes/interfaces.
        ///They are sent here by dependency injection system automatically.
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
            var query = _taskRepository.GetAll();

            if (input.AssignedPersonId.HasValue)
            {
                //query = query.Where(t => t.AssignedPersonId == input.AssignedPersonId.Value);
            }

            if (input.State.HasValue)
            {
                query = query.Where(t => t.State == input.State.Value);
            }

            //排序
            if (!string.IsNullOrEmpty(input.Sorting))
            {
                query = query.OrderBy(input.Sorting);
            }
            else
            {
                query = query.OrderByDescending(t => t.CreationTime);
            }
            //获取分页
            var taskList =
                query.Skip(input.SkipCount).Take(input.MaxResultCount).Include(t => t.AssignedPerson).ToList();

            //Used AutoMapper to automatically convert List<Task> to List<TaskDto>.
            return new GetTasksOutput
            {
                Tasks = Mapper.Map<List<TaskDto>>(taskList)
            };
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

            //Creating a new Task entity with given input's properties
            //var task = new Task
            //{
            //    Description = input.Description,
            //    Title = input.Title,
            //    State = input.State,
            //    CreationTime = Clock.Now
            //};

            if (input.AssignedPersonId.HasValue)
            {
                task.AssignedPerson = _userRepository.Load(input.AssignedPersonId.Value);

                SmtpEmailSender emailSender = new SmtpEmailSender(_smtpEmialSenderConfig);
                string message = "You hava been assigned one task into your todo list.";
                emailSender.Send("ysjshengjie@qq.com", task.AssignedPerson.EmailAddress, "New Todo item", message);
                _notificationPublisher.Publish("ADDTask");
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
            {
                _taskRepository.Delete(task);
            }
        }
    }
}