using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EntityFramework.DynamicFilters;
using LearningMpaAbp.EntityFramework;

namespace LearningMpaAbp.Tests.TestDatas
{
    public class TestDataBuilder
    {
        private readonly LearningMpaAbpDbContext _context;
        private readonly int _tenantId;

        public TestDataBuilder(LearningMpaAbpDbContext context, int tenantId)
        {
            _context = context;
            _tenantId = tenantId;
        }

        public void Create()
        {
            _context.DisableAllFilters();

            new TestUserBuilder(_context,_tenantId).Create();
            new TestTasksBuilder(_context,_tenantId).Create();

            _context.SaveChanges();
        }
    }
}
