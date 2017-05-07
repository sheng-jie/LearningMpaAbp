using Abp.Events.Bus;
using LearningMpaAbp.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LearningMpaAbp.Tasks
{
    public class TaskAssignedEventData : TaskEventData
    {
        public User User { get; set; }
        public TaskAssignedEventData(Task task, User user)
        {
            this.Task = task;
            this.User = user;
        }
    }
}
