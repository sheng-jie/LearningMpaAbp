using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.AutoMapper;

namespace LearningMpaAbp.Tasks.Dtos
{
    [AutoMapFrom(typeof(Task))]
    public class TaskCacheItem
    {
        public string Title { get; set; }

        public string Description { get; set; }

        public TaskState State { get; set; }
    }
}
