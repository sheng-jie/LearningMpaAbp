using System;
using System.ComponentModel.DataAnnotations;
using Abp.Runtime.Validation;
using Abp.AutoMapper;

namespace LearningMpaAbp.Tasks.Dtos
{
    /// <summary>
    /// This DTO class is used to send needed data to <see cref="ITaskAppService.UpdateTask"/> method.
    /// 
    /// Implements <see cref="ICustomValidate"/> for additional custom validation.
    /// </summary>
    public class UpdateTaskInput : ICustomValidate
    {
        [Range(1, Int32.MaxValue)] //Data annotation attributes work as expected.
        public int Id { get; set; }

        public long? AssignedPersonId { get; set; }

        public TaskState? State { get; set; }

        [Required]
        public string Title { get; set; }

        [Required]
        public string Description { get; set; }

        //Custom validation method. It's called by ABP after data annotation validations.
        public void AddValidationErrors(CustomValidationContext context)
        {
            if (AssignedPersonId == null && State == null)
            {
                context.Results.Add(new ValidationResult("Both of AssignedPersonId and State can not be null in order to update a Task!", new[] { "AssignedPersonId", "State" }));
            }
        }

        public override string ToString()
        {
            return string.Format("[UpdateTaskInput > TaskId = {0}, AssignedPersonId = {1}, State = {2}]", Id, AssignedPersonId, State);
        }
    }
}