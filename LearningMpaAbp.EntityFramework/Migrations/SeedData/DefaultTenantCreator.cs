using System.Linq;
using LearningMpaAbp.EntityFramework;
using LearningMpaAbp.MultiTenancy;

namespace LearningMpaAbp.Migrations.SeedData
{
    public class DefaultTenantCreator
    {
        private readonly LearningMpaAbpDbContext _context;

        public DefaultTenantCreator(LearningMpaAbpDbContext context)
        {
            _context = context;
        }

        public void Create()
        {
            CreateUserAndRoles();
        }

        private void CreateUserAndRoles()
        {
            //Default tenant

            var defaultTenant = _context.Tenants.FirstOrDefault(t => t.TenancyName == Tenant.DefaultTenantName);
            if (defaultTenant == null)
            {
                _context.Tenants.Add(new Tenant {TenancyName = Tenant.DefaultTenantName, Name = Tenant.DefaultTenantName});
                _context.SaveChanges();
            }
        }
    }
}
