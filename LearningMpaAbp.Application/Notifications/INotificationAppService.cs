using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Text;
using System.Threading.Tasks;
using Abp.Application.Services;

namespace LearningMpaAbp.Notifications
{
    public interface INotificationAppService : IApplicationService
    {
        void NotificationUsersWhoHaveOpenTask();
    }
}
