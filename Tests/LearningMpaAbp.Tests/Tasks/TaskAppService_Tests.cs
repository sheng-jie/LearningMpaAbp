using System.Linq;
using System.Net;
using Abp.Authorization;
using Abp.Domain.Repositories;
using Abp.Runtime.Validation;
using LearningMpaAbp.MultiTenancy;
using LearningMpaAbp.Tasks;
using LearningMpaAbp.Tasks.Dtos;
using LearningMpaAbp.Users;
using Microsoft.AspNet.Identity;
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
        public void Should_Create_New_Task_WithPermission()
        {
            //Arrange
            //LoginAsDefaultTenantAdmin();//基类的构造函数中已经以默认租户Admin登录。
            var initalCount = UsingDbContext(ctx => ctx.Tasks.Count());

            var task1 = new CreateTaskInput()
            {
                Title = "Test Task",
                Description = "Test Task",
                State = TaskState.Open
            };

            var task2 = new CreateTaskInput()
            {
                Title = "Test Task2",
                Description = "Test Task2",
                State = TaskState.Open
            };

            //Act
            int taskResult1 = _taskAppService.CreateTask(task1);
            int taskResult2 = _taskAppService.CreateTask(task2);


            //Assert
            UsingDbContext(ctx =>
            {
                taskResult1.ShouldBeGreaterThan(0);
                taskResult2.ShouldBeGreaterThan(0);
                ctx.Tasks.Count().ShouldBe(initalCount + 2);
                ctx.Tasks.FirstOrDefault(t => t.Title == "Test Task").ShouldNotBe(null);
                var task = ctx.Tasks.FirstOrDefault(t => t.Title == "Test Task2");
                task.ShouldNotBe(null);
                task.State.ShouldBe(TaskState.Open);
            });
        }

        /// <summary>
        /// 若没有分配任务给他人的权限，创建的任务指定给他人，则任务创建不成功。
        /// </summary>
        [Fact]
        public void Should_Not_Create_New_Order_AssignToOrther_WithoutPermission()
        {
            //Arrange
            LoginAsTenant(Tenant.DefaultTenantName, "TestUser");

            //获取admin用户
            var adminUser = UsingDbContext(ctx => ctx.Users.FirstOrDefault(u => u.UserName == User.AdminUserName));

            var newTask = new CreateTaskInput()
            {
                Title = "Test Task",
                Description = "Test Task",
                State = TaskState.Open,
                AssignedPersonId = adminUser.Id //TestUser创建Task并分配给Admin
            };

            //Act,Assert
            Assert.Throws<AbpAuthorizationException>(() => _taskAppService.CreateTask(newTask));

        }

    }
}