using System.Linq;
using System.Linq.Dynamic;
using System.Web.Mvc;
using LearningMpaAbp.Tasks;
using LearningMpaAbp.Tasks.Dtos;
using LearningMpaAbp.Web.Models.Tasks;
using System;
using System.Collections.Generic;
using LearningMpaAbp.Users;

namespace LearningMpaAbp.Web.Controllers
{
    public class BackendTasksController : LearningMpaAbpControllerBase
    {
        private readonly ITaskAppService _taskAppService;
        private readonly IUserAppService _userAppService;

        public BackendTasksController(ITaskAppService taskAppService, IUserAppService userAppService)
        {
            _taskAppService = taskAppService;
            _userAppService = userAppService;
        }

        // GET: Task
        public ActionResult List()
        {
            ViewBag.TaskStateDropdownList = GetTaskStateDropdownList(null);
            var userList = _userAppService.GetUsers();
            ViewBag.AssignedPersonId = new SelectList(userList.Items, "Id", "Name");
            return View();
        }


        public JsonResult GetAllTasks(int limit, int offset, string sortfiled, string sortway, string search, string status)
        {
            var output = _taskAppService.GetTasks(new GetTasksInput());

            TaskState currentState;

            var result = output.Tasks.Where(t => t.Title.Contains(search));
            if (!string.IsNullOrEmpty(sortfiled))
            {
                result = result.OrderBy(string.Format("{0} {1}", sortfiled, sortway));
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

            var userList = _userAppService.GetUsers();
            ViewBag.AssignedPersonId = new SelectList(userList.Items, "Id", "Name",updateTaskDto.AssignedPersonId);

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