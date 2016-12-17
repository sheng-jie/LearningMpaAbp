using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Linq.Dynamic;
using System.Net;
using System.Web;
using System.Web.Mvc;
using LearningMpaAbp.EntityFramework;
using LearningMpaAbp.Tasks;
using LearningMpaAbp.Tasks.Dtos;
using LearningMpaAbp.Web.Models.Tasks;

namespace LearningMpaAbp.Web.Controllers
{
    public class TasksController : Controller
    {
        private readonly ITaskAppService _taskAppService;

        public TasksController(ITaskAppService taskAppService)
        {
            this._taskAppService = taskAppService;
        }

        // GET: Tasks
        public ActionResult Index(GetTasksInput input)
        {
            var output = _taskAppService.GetTasks(input);
            var module = new IndexViewModel(output.Tasks)
            {
                SelectedTaskState = input.State

            };
            return View(module);
        }
        
        public PartialViewResult GetList(GetTasksInput input)
        {
            var output = _taskAppService.GetTasks(input);
            var module = new IndexViewModel(output.Tasks)
            {
                SelectedTaskState = input.State

            };
            return PartialView("_List", module);
        }

        [ChildActionOnly]
        public PartialViewResult Create()
        {
            return PartialView("_CreateTask");
        }
        
        public PartialViewResult RemoteCreate()
        {
            return PartialView("_CreateTaskPartial");
        }


        // POST: Tasks/Create
        // 为了防止“过多发布”攻击，请启用要绑定到的特定属性，有关 
        // 详细信息，请参阅 http://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(CreateTaskInput task)
        {
            var id = _taskAppService.CreateTask(task);

            var input = new GetTasksInput();
            var output = _taskAppService.GetTasks(input);
            var module = new IndexViewModel(output.Tasks)
            {
                SelectedTaskState = input.State
            };

            return PartialView("_List", module);
        }

        // GET: Tasks/Edit/5

        public PartialViewResult Edit(int id)
        {
            var task = _taskAppService.GetTaskById(id);

            var updateTaskDto = AutoMapper.Mapper.Map<UpdateTaskInput>(task);

            return PartialView("_EditTask", updateTaskDto);
        }

        // POST: Tasks/Edit/5
        // 为了防止“过多发布”攻击，请启用要绑定到的特定属性，有关 
        // 详细信息，请参阅 http://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(UpdateTaskInput updateTaskDto)
        {
            if (ModelState.IsValid)
            {

                _taskAppService.UpdateTask(updateTaskDto);

                var input = new GetTasksInput();
                var output = _taskAppService.GetTasks(input);
                var module = new IndexViewModel(output.Tasks)
                {
                    SelectedTaskState = input.State
                };

                return PartialView("_List", module);
            }
            return View("Index");
        }

        
    }
}
