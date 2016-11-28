using System.ComponentModel.DataAnnotations;

namespace LearningMpaAbp.Tasks.Dtos
{
    public class CreateTaskInput
    {
        public int? AssignedPersonId { get; set; }

        [Required]
        public string Description { get; set; }

        [Required]
        public string Title { get; set; }

        public TaskState State { get; set; }
        public override string ToString()
        {
            return string.Format("[CreateTaskInput > AssignedPersonId = {0}, Description = {1}]", AssignedPersonId, Description);
        }
    }
}