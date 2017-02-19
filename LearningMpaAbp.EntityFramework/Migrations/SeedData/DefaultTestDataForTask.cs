using System.Collections.Generic;
using System.Linq;
using LearningMpaAbp.EntityFramework;
using LearningMpaAbp.Tasks;

namespace LearningMpaAbp.Migrations.SeedData
{
    public class DefaultTestDataForTask
    {
        private readonly LearningMpaAbpDbContext _context;

        private static readonly List<Task> _tasks;

        public DefaultTestDataForTask(LearningMpaAbpDbContext context)
        {
            _context = context;
        }

        static DefaultTestDataForTask()
        {
            _tasks = new List<Task>()
            {
                new Task("Learning ABP deom", "Learning how to use abp framework to build a MPA application."),
                new Task("Make Lunch", "Cook 2 dishs")
            };
        }

        public void Create()
        {
            foreach (var task in _tasks)
            {
                if (_context.Tasks.FirstOrDefault(t => t.Title == task.Title) == null)
                {
                    _context.Tasks.Add(task);
                }
                _context.SaveChanges();
            }
        }

    }
}