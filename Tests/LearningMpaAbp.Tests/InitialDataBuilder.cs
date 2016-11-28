using System.Collections.Generic;
using System.Data.Entity.Migrations;
using LearningMpaAbp.EntityFramework;
using LearningMpaAbp.Tasks;

namespace LearningMpaAbp.Tests
{
    public class InitialDataBuilder
    {
        public void Build(LearningMpaAbpDbContext context)
        {
            var tasks = new List<Task>()
            {
                new Task("Learning ABP deom", "Learning how to use abp framework to build a MPA application."),
                new Task("Make Lunch", "Cook 2 dishs")
            };

            foreach (var task in tasks)
            {
                context.Tasks.AddOrUpdate(task);
            }

            context.SaveChanges();
        }
    }
}