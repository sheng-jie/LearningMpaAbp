using System.Reflection;
using Abp.AutoMapper;
using Abp.Modules;
using Castle.MicroKernel.Registration;

namespace LearningMpaAbp
{
    [DependsOn(typeof(LearningMpaAbpCoreModule), typeof(AbpAutoMapperModule))]
    public class LearningMpaAbpApplicationModule : AbpModule
    {
        public override void PreInitialize()
        {
            Configuration.Modules.AbpAutoMapper().Configurators.Add(mapper =>
            {
                //Add your custom AutoMapper mappings here...
            });
        }

        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(Assembly.GetExecutingAssembly());

            //注册IDtoMapping
            IocManager.IocContainer.Register(
                Classes.FromAssembly(Assembly.GetExecutingAssembly())
                    .IncludeNonPublicTypes()
                    .BasedOn<IDtoMapping>()
                    .WithService.Self()
                    .WithService.DefaultInterfaces()
                    .LifestyleTransient()
            );

            //解析依赖，并进行映射规则创建
            Configuration.Modules.AbpAutoMapper().Configurators.Add(mapper =>
            {
                var mappers = IocManager.IocContainer.ResolveAll<IDtoMapping>();
                foreach (var dtomap in mappers)
                    dtomap.CreateMapping(mapper);
            });
        }
    }
}