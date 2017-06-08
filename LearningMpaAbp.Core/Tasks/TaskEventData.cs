using Abp.Events.Bus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LearningMpaAbp.Tasks
{
    public class TaskEventData : EventData
    {
        public Task Task { get; set; }
    }
}
