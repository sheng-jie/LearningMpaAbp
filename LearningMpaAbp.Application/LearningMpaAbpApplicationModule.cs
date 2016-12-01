using System.Reflection;
using Abp.AutoMapper;
using Abp.Modules;
using LearningMpaAbp.Tasks.Dtos;
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
                //mapper.CreateMap<,>()
                //mapper.CreateMap<CreateTaskInput, Tasks.Task>();
                //mapper.CreateMap<TaskDto, UpdateTaskInput>();
                //mapper.CreateMap<UpdateTaskInput, Tasks.Task>();
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

            Configuration.Modules.AbpAutoMapper().Configurators.Add(mapper =>
            {
                var mappers = IocManager.IocContainer.ResolveAll<IDtoMapping>();
                foreach (var dtomap in mappers)
                {
                    dtomap.CreateMapping(mapper);
                }
            });
        }
    }
}
