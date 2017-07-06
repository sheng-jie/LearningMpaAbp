using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.Notifications;

namespace LearningMpaAbp.Jobs
{
    [Serializable]
    public class SendNotificationJobArgs
    {
        public long TargetUserId { get; set; }

        public string NotificationTitle { get; set; }

        public MessageNotificationData NotificationData { get; set; }

        public NotificationSeverity NotificationSeverity { get; set; }
    }
}
