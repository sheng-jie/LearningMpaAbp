using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LearningMpaAbp.EntityFramework;
using LearningMpaAbp.Tasks;

namespace LearningMpaAbp.Tests.TestDatas
{
    public class TestTasksBuilder
    {
        private readonly LearningMpaAbpDbContext _context;
        private readonly int _tenantId;

        public TestTasksBuilder(LearningMpaAbpDbContext context, int tenantId)
        {
            _context = context;
            _tenantId = tenantId;
        }

        public void Create()
        {
            for (int i = 0; i < 8; i++)
            {
                var task = new Task()
                {
                    Title = "TestTask" + i,
                    Description = "Test Task " + i,
                    CreationTime = DateTime.Now,
                    State = (TaskState)new Random().Next(0, 1)
                };
                _context.Tasks.Add(task);
            }
            
        }
    }
}
