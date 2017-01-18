using LearningMpaAbp.Dto;

namespace LearningMpaAbp.Tasks.Dtos
{
    public class GetTasksInput : PagedAndSortedInputDto
    {
        public TaskState? State { get; set; }

        public int? AssignedPersonId { get; set; }

        public string TaskTitle { get; set; }
    }
}