using LearningMpaAbp.Dto;

namespace LearningMpaAbp.Tasks.Dtos
{
    public class GetTasksInput : PagedSortedAndFilteredInputDto
    {
        public TaskState? State { get; set; }

        public int? AssignedPersonId { get; set; }
    }
}