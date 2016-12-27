using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Linq.Dynamic;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Abp.Application.Services.Dto;
using Abp.Threading;
using LearningMpaAbp.EntityFramework;
using LearningMpaAbp.Tasks;
using LearningMpaAbp.Tasks.Dtos;
using LearningMpaAbp.Web.Models.Tasks;
using X.PagedList;
using AutoMapper;
using LearningMpaAbp.Users;
using LearningMpaAbp.Users.Dto;
using Abp.Web.Mvc.Authorization;

namespace LearningMpaAbp.Web.Controllers
{
    [AbpMvcAuthorize]
    public class TasksController : Controller
    {
        private readonly ITaskAppService _taskAppService;
        private readonly IUserAppService _userAppService;

        public TasksController(ITaskAppService taskAppService, IUserAppService userAppService)
        {
            this._taskAppService = taskAppService;
            _userAppService = userAppService;
        }

        public ActionResult Index(GetTasksInput input)
        {
            var output = _taskAppService.GetTasks(input);

            var model = new IndexViewModel(output.Tasks)
            {
                SelectedTaskState = input.State

            };
            return View(model);
        }

        // GET: Tasks
        public ActionResult PagedList(int? page)
        {
            var tasks = _taskAppService.GetAllTasks();
            //if (state.HasValue)
            //{
            //    tasks = tasks.Where(t => t.State == state.Value);
            //}


            var pageNumber = page ?? 1;

            var onePageOfTasks = tasks.ToPagedList(pageNumber, 5); // will only contain 25 products max because of the pageSize


            ViewBag.OnePageOfTasks = onePageOfTasks;

            return View();
        }


        public PartialViewResult GetList(GetTasksInput input)
        {
            var output = _taskAppService.GetTasks(input);

            var model = new IndexViewModel(output.Tasks)
            {
                SelectedTaskState = input.State

            };
            return PartialView("_List", model);
        }

        [ChildActionOnly]
        public PartialViewResult Create()
        {
            var userList = _userAppService.GetUsers();
            ViewBag.AssignedPersonId = new SelectList(userList.Items, "Id", "Name");
            return PartialView("_CreateTask");
        }

        public PartialViewResult RemoteCreate()
        {
            var userList = _userAppService.GetUsers();
            ViewBag.AssignedPersonId = new SelectList(userList.Items, "Id", "Name");
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
            var model = new IndexViewModel(output.Tasks)
            {
                SelectedTaskState = input.State
            };

            return PartialView("_List", model);
        }

        // GET: Tasks/Edit/5

        public PartialViewResult Edit(int id)
        {
            var task = _taskAppService.GetTaskById(id);

            var updateTaskDto = AutoMapper.Mapper.Map<UpdateTaskInput>(task);

            var userList = _userAppService.GetUsers();
            ViewBag.AssignedPersonId = new SelectList(userList.Items, "Id", "Name", updateTaskDto.AssignedPersonId);

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
                var model = new IndexViewModel(output.Tasks)
                {
                    SelectedTaskState = input.State
                };

                return PartialView("_List", model);
            }
            return View("Index");
        }


    }
}
