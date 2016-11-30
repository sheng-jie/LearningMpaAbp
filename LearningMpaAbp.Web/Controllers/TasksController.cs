using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
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
                SelectedTaskState = input.State,
                CreateTaskInput = new CreateTaskInput()

            };
            return View(module);
        }

        
        public PartialViewResult GetList(GetTasksInput input)
        {
            var output = _taskAppService.GetTasks(input);
            var module = new IndexViewModel(output.Tasks)
            {
                SelectedTaskState = input.State,
                CreateTaskInput = new CreateTaskInput()

            };
            return PartialView("_List", module);
        }

        //// GET: Tasks/Details/5
        //public ActionResult Details(int? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    var task = _taskAppService.GetTaskByIdAsync(id);
        //    if (task == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return System.Web.UI.WebControls.View(task);
        //}

        // GET: Tasks/Create
        public PartialViewResult Create()
        {
            return PartialView("_CreateTask");
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
                SelectedTaskState = input.State,
                CreateTaskInput = new CreateTaskInput()

            };

            return PartialView("_List", module);
            //var input = new GetTasksInput() {State = TaskState.Open};
            //var output = _taskAppService.GetTasks(input);
            //var module = new IndexViewModel(output.Tasks)
            //{
            //    SelectedTaskState = input.State
            //};

            //return View("Index", module);
        }

        // GET: Tasks/Edit/5
        public PartialViewResult Edit(int? id)
        {
            var task = _taskAppService.GetTaskById(id.Value);

            return PartialView("_EditTask", task);
        }

        // POST: Tasks/Edit/5
        // 为了防止“过多发布”攻击，请启用要绑定到的特定属性，有关 
        // 详细信息，请参阅 http://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(TaskDto task)
        {
            var updateTaskDto = AutoMapper.Mapper.Map<UpdateTaskInput>(task);
            _taskAppService.UpdateTask(updateTaskDto);

            var input = new GetTasksInput();
            var output = _taskAppService.GetTasks(input);
            var module = new IndexViewModel(output.Tasks)
            {
                SelectedTaskState = input.State,
                CreateTaskInput = new CreateTaskInput()

            };

            return PartialView("_List", module);
        }

        //// GET: Tasks/Delete/5
        //public ActionResult Delete(int? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    Task task = db.Tasks.Find(id);
        //    if (task == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(task);
        //}

        //// POST: Tasks/Delete/5
        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        //public ActionResult DeleteConfirmed(int id)
        //{
        //    Task task = db.Tasks.Find(id);
        //    db.Tasks.Remove(task);
        //    db.SaveChanges();
        //    return RedirectToAction("Index");
        //}

        //protected override void Dispose(bool disposing)
        //{
        //    if (disposing)
        //    {
        //        db.Dispose();
        //    }
        //    base.Dispose(disposing);
        //}
    }
}
