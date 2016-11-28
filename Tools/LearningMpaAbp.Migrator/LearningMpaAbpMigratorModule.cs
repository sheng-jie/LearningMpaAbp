using System.Data.Entity;
using System.Reflection;
using Abp.Modules;
using LearningMpaAbp.EntityFramework;

namespace LearningMpaAbp.Migrator
{
    [DependsOn(typeof(LearningMpaAbpDataModule))]
    public class LearningMpaAbpMigratorModule : AbpModule
    {
        public override void PreInitialize()
        {
            Database.SetInitializer<LearningMpaAbpDbContext>(null);

            Configuration.BackgroundJobs.IsJobExecutionEnabled = false;
        }

        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(Assembly.GetExecutingAssembly());
        }
    }
}