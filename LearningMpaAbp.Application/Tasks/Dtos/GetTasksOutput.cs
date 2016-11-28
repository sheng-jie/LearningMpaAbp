using System.Collections.Generic;

namespace LearningMpaAbp.Tasks.Dtos
{
    public class GetTasksOutput
    {
        public List<TaskDto> Tasks { get; set; }
    }
}