using System.Linq;
using System.Linq.Dynamic;
using System.Web.Mvc;
using LearningMpaAbp.Tasks;
using LearningMpaAbp.Tasks.Dtos;
using LearningMpaAbp.Web.Models.Tasks;
using System;
using System.Collections.Generic;

namespace LearningMpaAbp.Web.Controllers
{
    public class TaskController : LearningMpaAbpControllerBase
    {
        private readonly ITaskAppService _taskAppService;

        public TaskController(ITaskAppService taskAppService)
        {
            this._taskAppService = taskAppService;
        }

        // GET: Task
        public ActionResult List()
        {
            ViewBag.TaskStateDropdownList = GetTaskStateDropdownList(null);
            return View();
        }


        public JsonResult GetAllTasks(int limit, int offset, string order, string ordername, string search, string status)
        {
            var output = _taskAppService.GetTasks(new GetTasksInput());

            TaskState currentState;

            var result = output.Tasks.Where(t => t.Title.Contains(search));
            if (!string.IsNullOrEmpty(ordername))
            {
                result = result.OrderBy(ordername);
            }
            if (!string.IsNullOrEmpty(status))
            {
                if (Enum.TryParse<TaskState>(status, true, out currentState))
                    result = result.Where(r => r.State == currentState);
            }

            var taskDtos = result as TaskDto[] ?? result.ToArray();
            var total = taskDtos.ToList().Count;

            var rows = taskDtos.Skip(offset).Take(limit).ToList();

            

            return AbpJson(new { total = total, rows = rows }, wrapResult: false, camelCase: false, behavior: JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult Create(CreateTaskInput task)
        {
            var id = _taskAppService.CreateTask(task);

            return Json(id, JsonRequestBehavior.AllowGet);
        }
        public PartialViewResult Edit(int id)
        {
            var task = _taskAppService.GetTaskById(id);

            var updateTaskDto = AutoMapper.Mapper.Map<UpdateTaskInput>(task);

            return PartialView("_EditTask", updateTaskDto);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult Edit(UpdateTaskInput updateTaskDto)
        {
            if (ModelState.IsValid)
            {

                _taskAppService.UpdateTask(updateTaskDto);

                var input = new GetTasksInput();
                var output = _taskAppService.GetTasks(input);
                var module = new IndexViewModel(output.Tasks)
                {
                    SelectedTaskState = input.State,
                    CreateTaskInput = new CreateTaskInput(),
                    UpdateTaskInput = new UpdateTaskInput()
                };

                return Json(true, JsonRequestBehavior.AllowGet);
            }
            return Json(false, JsonRequestBehavior.AllowGet);

        }

        private List<SelectListItem> GetTaskStateDropdownList(TaskState? curState)
        {
            var list = new List<SelectListItem>()
            {
                new SelectListItem()
                {
                    Text = "AllTasks",
                    Value = "",
                    Selected = curState==null
                }
            };

            list.AddRange(Enum.GetValues(typeof(TaskState))
                .Cast<TaskState>()
                .Select(state => new SelectListItem()
                {
                    Text = $"TaskState_{state}",
                    Value = state.ToString(),
                    Selected = state == curState
                })
            );

            return list;
        }
    }
}