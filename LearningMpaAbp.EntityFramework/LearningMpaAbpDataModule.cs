using System.Data.Entity;
using System.Reflection;
using Abp.Modules;
using Abp.Zero.EntityFramework;
using LearningMpaAbp.EntityFramework;

namespace LearningMpaAbp
{
    [DependsOn(typeof(AbpZeroEntityFrameworkModule), typeof(LearningMpaAbpCoreModule))]
    public class LearningMpaAbpDataModule : AbpModule
    {
        public override void PreInitialize()
        {
            Database.SetInitializer(new CreateDatabaseIfNotExists<LearningMpaAbpDbContext>());

            Configuration.DefaultNameOrConnectionString = "Default";
        }

        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(Assembly.GetExecutingAssembly());
        }
    }
}
