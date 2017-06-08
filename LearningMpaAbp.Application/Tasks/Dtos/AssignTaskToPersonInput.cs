namespace LearningMpaAbp.Tasks.Dtos
{
    public class AssignTaskToPersonInput
    {
        public int TaskId { get; set; }

        public long UserId { get; set; }
    }
}