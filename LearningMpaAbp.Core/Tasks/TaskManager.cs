using Abp.Domain.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LearningMpaAbp.Users;
using Abp.Domain.Repositories;
using Abp.Authorization;
using LearningMpaAbp.Authorization;
using Abp.Runtime.Session;

namespace LearningMpaAbp.Tasks
{
    public class TaskManager : DomainService, ITaskManager
    {
        private readonly IPermissionChecker _permissionChecker;
        private readonly IAbpSession _abpSession;

        public TaskManager(IPermissionChecker permissionChecker, IAbpSession abpSession)
        {
            _permissionChecker = permissionChecker;
            _abpSession = abpSession;
        }

        public void AssignTaskToPerson(Task task, User user)
        {
            //已经分配，就不再分配
            if (task.AssignedPersonId.HasValue && task.AssignedPersonId.Value == user.Id)
            {
                return;
            }

            if (task.State != TaskState.Open)
            {
                throw new ApplicationException("处于非活动状态的任务不能分配！");
            }

            //获取是否有【分配任务给他人】的权限
            bool canAssignTaskToOther = _permissionChecker.IsGranted(PermissionNames.Pages_Tasks_AssignPerson);
            if (user.Id != _abpSession.GetUserId() && !canAssignTaskToOther)
            {
                throw new AbpAuthorizationException("没有分配任务给他人的权限！");
            }
            

            task.AssignedPersonId = user.Id;
        }
    }
}
