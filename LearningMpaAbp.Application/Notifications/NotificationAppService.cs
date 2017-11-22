using System.Data.Entity;
using System.Linq;
using Abp.BackgroundJobs;
using Abp.Domain.Repositories;
using Abp.Notifications;
using LearningMpaAbp.Jobs;
using LearningMpaAbp.Tasks;

namespace LearningMpaAbp.Notifications
{
    public class NotificationAppService : LearningMpaAbpAppServiceBase, INotificationAppService
    {
        private readonly IRepository<Task> _taskRepository;
        private readonly IBackgroundJobManager _backgroundJobManager;

        public NotificationAppService(IRepository<Task> taskRepository, IBackgroundJobManager backgroundJobManager)
        {
            _taskRepository = taskRepository;
            _backgroundJobManager = backgroundJobManager;
        }
        public void NotificationUsersWhoHaveOpenTask()
        {
            var openTasks = _taskRepository.GetAll().AsNoTracking().Where(t => t.State == TaskState.Open);
            foreach (var openTask in openTasks)
            {
                if (!openTask.AssignedPersonId.HasValue)
                {
                    continue;
                }

                var sendNotificationArgs = new SendNotificationJobArgs()
                {
                    NotificationTitle = "You have an open task",
                    NotificationSeverity = NotificationSeverity.Info,
                    NotificationData = new MessageNotificationData(openTask.Title),
                    TargetUserId = openTask.AssignedPersonId.Value
                };

                _backgroundJobManager.Enqueue<SendNotificationJob, SendNotificationJobArgs>(sendNotificationArgs);
            }
        }
    }
}