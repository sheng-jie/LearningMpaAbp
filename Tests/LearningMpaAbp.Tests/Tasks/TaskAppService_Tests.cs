using System.Linq;
using System.Net;
using Abp.Authorization;
using Abp.Runtime.Validation;
using LearningMpaAbp.MultiTenancy;
using LearningMpaAbp.Tasks;
using LearningMpaAbp.Tasks.Dtos;
using Shouldly;
using Xunit;

namespace LearningMpaAbp.Tests.Tasks
{
    public class TaskAppService_Tests : LearningMpaAbpTestBase
    {
        private readonly ITaskAppService _taskAppService;

        public TaskAppService_Tests()
        {
            _taskAppService = Resolve<TaskAppService>();
        }

        [Fact]
        public void Should_Create_New_Order()
        {
            LoginAsDefaultTenantAdmin();
            var initalCount = UsingDbContext(ctx => ctx.Tasks.Count());

            _taskAppService.CreateTask(new CreateTaskInput()
            {
                Title = "Test Task",
                Description = "Test Task",
                State = TaskState.Open
            });

            _taskAppService.CreateTask(new CreateTaskInput()
            {
                Title = "Test Task2",
                Description = "Test Task2",
                State = TaskState.Open
            });

            UsingDbContext(ctx =>
            {
                ctx.Tasks.Count().ShouldBe(initalCount + 2);
                ctx.Tasks.FirstOrDefault(t => t.Title == "Test Task").ShouldNotBe(null);
                var task2 = ctx.Tasks.FirstOrDefault(t => t.Title == "Test Task2");
                task2.ShouldNotBe(null);
                task2.State.ShouldBe(TaskState.Open);
            });
        }

        /// <summary>
        /// 若没有分配任务给他人的权限，创建的任务指定给他人，则任务创建不成功。
        /// </summary>
        [Fact]
        public void Should_Not_Create_New_Order_AssignToOrther_WithoutPermission()
        {
            LoginAsTenant(Tenant.DefaultTenantName, "TestUser");
            
            _taskAppService.CreateTask(new CreateTaskInput()
            {
                Title = "Test Task",
                Description = "Test Task",
                State = TaskState.Open,
                AssignedPersonId = 4
            });

            Assert.Throws<AbpAuthorizationException>(() => _taskAppService.CreateTask(new CreateTaskInput()
            {
                Title = "Test Task",
                Description = "Test Task",
                State = TaskState.Open,
                AssignedPersonId = 4
            }));


        }
        
    }
}