using Abp.Dependency;
using Abp.Events.Bus.Handlers;
using Abp.Net.Mail.Smtp;
using Abp.Notifications;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LearningMpaAbp.Tasks
{
    public class TaskAssignedToUser : IEventHandler<TaskAssignedEventData>, ITransientDependency
    {
        private readonly ISmtpEmailSender _smtpEmailSender;
        private readonly INotificationPublisher _notificationPublisher;
        public TaskAssignedToUser(ISmtpEmailSender smtpEmailSender, INotificationPublisher notificationPublisher)
        {
            _smtpEmailSender = smtpEmailSender;
            _notificationPublisher = notificationPublisher;
        }
        public void HandleEvent(TaskAssignedEventData eventData)
        {
            var message = "You hava been assigned one task into your todo list.";
            //TODO:需要重新配置QQ邮箱密码
            //_smtpEmailSender.Send("ysjshengjie@qq.com", eventData.Task.AssignedPerson.EmailAddress, "New Todo item", message);

            _notificationPublisher.Publish("NewTask", new MessageNotificationData(message), null,
                        NotificationSeverity.Info, new[] { eventData.User.ToUserIdentifier() });
        }
    }
}
