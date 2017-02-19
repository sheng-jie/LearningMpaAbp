using System.Collections.Generic;
using System.Linq;
using System.Net;
using Abp.Authorization;
using Abp.Domain.Repositories;
using Abp.Runtime.Validation;
using AutoMapper;
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
        public void Should_Create_New_Task_With_Permission()
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
        public void Should_Not_Create_New_Order_AssignToOrther_Without_Permission()
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

        [Fact]
        public void Should_Get_All_Tasks()
        {
            //Arrange
            var totalCount = UsingDbContext(ctx => ctx.Tasks.Count());

            var actualCount = _taskAppService.GetAllTasks().Count;

            actualCount.ShouldBe(totalCount);
        }

        [Fact]
        public void Should_Get_Filtered_Task()
        {
            //Arrange
            var filter = new GetTasksInput()
            {
                State = TaskState.Open
            };

            var openTasksCount = UsingDbContext(ctx => ctx.Tasks.Count(t => t.State == TaskState.Open));

            //Act
            var actualOpenTaskCount = _taskAppService.GetTasks(filter).Tasks.Count;

            //Assert
            actualOpenTaskCount.ShouldBe(openTasksCount);
        }

        [Fact]
        public void Should_Get_Paged_Task()
        {
            //Arrange（取第三页）
            var filter = new GetTasksInput()
            {
                MaxResultCount = 2,
                SkipCount = 4
            };
            var totalTaskCount = UsingDbContext(ctx => ctx.Tasks.Count());
            var thirdPageItemsCount = totalTaskCount > 3 * filter.MaxResultCount ? filter.MaxResultCount : totalTaskCount - filter.SkipCount;

            //act
            var result = _taskAppService.GetPagedTasks(filter);

            //Assert
            result.TotalCount.ShouldBe(totalTaskCount);
            result.Items.Count.ShouldBe(thirdPageItemsCount);


        }

        [Fact]
        public async void Should_Get_Task_By_Id()
        {
            //Arrange
            var defaultTask = UsingDbContext(ctx => ctx.Tasks.FirstOrDefault());

            //Act

            var task = _taskAppService.GetTaskById(defaultTask.Id);
            var task2 = await _taskAppService.GetTaskByIdAsync(defaultTask.Id);

            //Assert

            task.ShouldNotBeNull();
            task.Title.ShouldBeSameAs(defaultTask.Title);
            task2.ShouldNotBeNull();
            task2.Title.ShouldBeSameAs(defaultTask.Title);
        }

        [Fact]
        public void Should_Assign_Task_To_Another_With_Permission()
        {
            //Arrange 
            var defaultTask = UsingDbContext(ctx => ctx.Tasks.FirstOrDefault());
            var testUser = UsingDbContext(ctx => ctx.Users.FirstOrDefault(u => u.UserName == "TestUser"));
            var updateTask = new UpdateTaskInput()
            {
                Title = defaultTask.Title,
                Description = defaultTask.Description,
                Id = defaultTask.Id,
                State = defaultTask.State,
                AssignedPersonId = defaultTask.AssignedPersonId
            };

            updateTask.Description = "update task";
            updateTask.AssignedPersonId = testUser.Id;
            //Act
            _taskAppService.UpdateTask(updateTask);

            var updatedTask = _taskAppService.GetTaskById(updateTask.Id);

            //Assert
            updatedTask.Id.ShouldBe(updatedTask.Id);
            updatedTask.Description.ShouldBe(updatedTask.Description);
            updatedTask.AssignedPersonId.ShouldBe(testUser.Id);
        }

        [Fact]
        public void Should_Not_Assign_Task_To_Other()
        {
            //Arrange
            LoginAsTenant(Tenant.DefaultTenantName, "TestUser");

            //获取admin用户
            var adminUser = UsingDbContext(ctx => ctx.Users.FirstOrDefault(u => u.UserName == User.AdminUserName));

            var defaultTask = UsingDbContext(ctx => ctx.Tasks.FirstOrDefault());
            var updateTask = new UpdateTaskInput()
            {
                Title = defaultTask.Title,
                Description = defaultTask.Description,
                Id = defaultTask.Id,
                State = defaultTask.State,
                AssignedPersonId = defaultTask.AssignedPersonId
            };
            updateTask.AssignedPersonId = adminUser.Id;
            

            //Act,Assert

            Assert.Throws<AbpAuthorizationException>(() => _taskAppService.UpdateTask(updateTask));
        }

        [Fact]
        public void Should_Delete_Task()
        {
            //Arrange
            var defaultTask = UsingDbContext(ctx => ctx.Tasks.FirstOrDefault());

            //Act
            _taskAppService.DeleteTask(defaultTask.Id);

            //Assert

            var task = UsingDbContext(ctx => ctx.Tasks.FirstOrDefault(t => t.Id == defaultTask.Id));
            task.ShouldBeNull();
        }
    }
}