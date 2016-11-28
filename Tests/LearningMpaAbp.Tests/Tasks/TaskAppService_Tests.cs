using LearningMpaAbp.Tasks;
using LearningMpaAbp.Tasks.Dtos;
using Shouldly;
using Xunit;

namespace LearningMpaAbp.Tests.Tasks
{
    public class TaskAppService_Tests:LearningMpaAbpTestBase
    {
        private readonly ITaskAppService _taskAppService;

        public TaskAppService_Tests()
        {
            _taskAppService = Resolve<TaskAppService>();
        }

        [Fact]
        public void Should_Get_All_Tasks()
        {
            var output = _taskAppService.GetTasks(new GetTasksInput());
            output.Tasks.Count.ShouldBe(2);
        }

        [Fact]
        public void Should_Get_All_Filtered_Tasks()
        {
            var output = _taskAppService.GetTasks(new GetTasksInput() {State = TaskState.Open});
            output.Tasks.ShouldAllBe(t=>t.State==0);
        }
    }
}