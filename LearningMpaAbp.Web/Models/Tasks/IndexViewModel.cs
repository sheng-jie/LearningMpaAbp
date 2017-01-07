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
        /// <summary>
        /// 用来进行绑定列表过滤状态
        /// </summary>
        public TaskState? SelectedTaskState { get; set; }

        /// <summary>
        /// 列表展示
        /// </summary>
        public IReadOnlyList<TaskDto> Tasks { get; }

        /// <summary>
        /// 创建任务模型
        /// </summary>
        public CreateTaskInput CreateTaskInput { get; set; }

        /// <summary>
        /// 更新任务模型
        /// </summary>
        public UpdateTaskInput UpdateTaskInput { get; set; }

        public IndexViewModel(IReadOnlyList<TaskDto> items)
        {
            Tasks = items;
        }
        
        /// <summary>
        /// 用于过滤下拉框的绑定
        /// </summary>
        /// <returns></returns>

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