using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.BackgroundJobs;
using Abp.Dependency;
using Abp.Domain.Repositories;
using Abp.Notifications;
using LearningMpaAbp.Users;

namespace LearningMpaAbp.Jobs
{
    public class SendNotificationJob : BackgroundJob<SendNotificationJobArgs>, ITransientDependency
    {
        private readonly IRepository<User,long> _userRepository;
        private readonly INotificationPublisher _notificationPublisher;

        public SendNotificationJob(IRepository<User, long> userRepository, INotificationPublisher notificationPublisher)
        {
            _userRepository = userRepository;
            _notificationPublisher = notificationPublisher;
        }

        public override void Execute(SendNotificationJobArgs args)
        {
            var targetUser = _userRepository.Get(args.TargetUserId);

            _notificationPublisher.Publish(args.NotificationTitle, args.NotificationData, null,
                args.NotificationSeverity, new[] {targetUser.ToUserIdentifier()});
        }
    }
}
