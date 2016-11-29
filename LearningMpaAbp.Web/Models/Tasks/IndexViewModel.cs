using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Abp.Localization;
using LearningMpaAbp.Tasks;
using LearningMpaAbp.Tasks.Dtos;

namespace LearningMpaAbp.Web.Models.Tasks
{
    public class IndexViewModel
    {
        public TaskState? SelectedTaskState { get; set; }
        public IReadOnlyList<TaskDto> Tasks { get; }

        public CreateTaskInput CreateTaskInput { get; set; }

        public IndexViewModel(IReadOnlyList<TaskDto> items)
        {
            Tasks = items;
        }

        public string GetTaskLable(TaskDto task)
        {
            string style = "";
            TaskState state = (TaskState) Enum.Parse(typeof(TaskState),task.State.ToString());

            switch (state)
            {
                case TaskState.Open:
                    style = "label-success";
                    break;
                case TaskState.Completed:
                    style = "label-default";
                    break;
            }
            return style;

        }

        public List<SelectListItem> GetTaskStateSelectListItems()
        {
            var list=new List<SelectListItem>()
            {
                new SelectListItem()
                {
                    Text = "AllTasks",
                    Value = "",
                    Selected = SelectedTaskState==null
                }
            };

            list.AddRange(Enum.GetValues(typeof(TaskState))
                .Cast<TaskState>()
                .Select(state=>new SelectListItem()
                {
                    Text = $"TaskState_{state}",
                    Value = state.ToString(),
                    Selected = state==SelectedTaskState
                })
            );

            return list;
        }
    }
}