using System;
using Abp.Application.Services.Dto;
using Abp.AutoMapper;

namespace LearningMpaAbp.Tasks.Dtos
{
    /// <summary>
    /// A DTO class that can be used in various application service methods when needed to send/receive Task objects.
    /// </summary>
    //[AutoMapFrom(typeof(Task))]
    public class TaskDto : EntityDto
    {
        public long? AssignedPersonId { get; set; }

        public string AssignedPersonName { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public DateTime CreationTime { get; set; }

        public TaskState State { get; set; }

        //This method is just used by the Console Application to list tasks
        public override string ToString()
        {
            return string.Format(
                "[Task Id={0}, Description={1}, CreationTime={2}, AssignedPersonName={3}, State={4}]",
                Id,
                Description,
                CreationTime,
                AssignedPersonId,
                (TaskState)State
                );
        }
    }
}