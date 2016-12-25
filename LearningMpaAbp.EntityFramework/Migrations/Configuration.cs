using System.Data.Entity.Migrations;
using Abp.MultiTenancy;
using Abp.Zero.EntityFramework;
using LearningMpaAbp.Migrations.SeedData;
using EntityFramework.DynamicFilters;

namespace LearningMpaAbp.Migrations
{
    public sealed class Configuration : DbMigrationsConfiguration<LearningMpaAbp.EntityFramework.LearningMpaAbpDbContext>, IMultiTenantSeed
    {
        public AbpTenantBase Tenant { get; set; }

        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
            ContextKey = "LearningMpaAbp";
        }

        protected override void Seed(LearningMpaAbp.EntityFramework.LearningMpaAbpDbContext context)
        {
            context.DisableAllFilters();

            if (Tenant == null)
            {
                //Host seed
                new InitialHostDbBuilder(context).Create();

                //Default tenant seed (in host database).
                new DefaultTenantCreator(context).Create();
                new TenantRoleAndUserBuilder(context, 1).Create();

                new DefaultTestDataForTask(context).Create();
            }
            else
            {
                //You can add seed for tenant databases and use Tenant property...
            }

            context.SaveChanges();
        }
    }
}
