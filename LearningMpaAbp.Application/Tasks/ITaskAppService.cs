using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Abp.Linq.Extensions;
using LearningMpaAbp.Tasks.Dtos;


namespace LearningMpaAbp.Tasks
{
    /// <summary>
    /// Defines an application service for <see cref="Task"/> operations.
    /// 
    /// It extends <see cref="IApplicationService"/>.
    /// Thus, ABP enables automatic dependency injection, validation and other benefits for it.
    /// 
    /// Application services works with DTOs (Data Transfer Objects).
    /// Service methods gets and returns DTOs.
    /// </summary>
    public interface ITaskAppService : IApplicationService
    {
        GetTasksOutput GetTasks(GetTasksInput input);

        PagedResultDto<TaskDto> GetPagedTasks(GetTasksInput input);

        void UpdateTask(UpdateTaskInput input);

        int CreateTask(CreateTaskInput input);

        Task<TaskDto> GetTaskByIdAsync(int taskId);

        TaskDto GetTaskById(int taskId);

        void DeleteTask(int taskId);

        TaskCacheItem GetTaskFromCacheById(int taskId);

        IList<TaskDto> GetAllTasks();
    }
}