using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LearningMpaAbp.EntityFramework;
using LearningMpaAbp.Users;

namespace LearningMpaAbp.Tests.TestDatas
{
    /// <summary>
    /// 预置测试用户（无权限）
    /// </summary>
    public class TestUserBuilder
    {
        private readonly LearningMpaAbpDbContext _context;
        private readonly int _tenantId;

        public TestUserBuilder(LearningMpaAbpDbContext context, int tenantId)
        {
            _context = context;
            _tenantId = tenantId;
        }

        public void Create()
        {
            var testUser =
                _context.Users.FirstOrDefault(u => u.TenantId == _tenantId && u.UserName == "TestUser");
            if (testUser == null)
            {
                testUser = new User
                {
                    TenantId = _tenantId,
                    UserName = "TestUser",
                    Name = "Test User",
                    Surname = "Test",
                    EmailAddress = "test@defaulttenant.com",
                    Password = User.DefaultPassword,
                    IsEmailConfirmed = true,
                    IsActive = true
                };

                _context.Users.Add(testUser);
            }

        }
    }
}
