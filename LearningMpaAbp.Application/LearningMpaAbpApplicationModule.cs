using System.Reflection;
using Abp.AutoMapper;
using Abp.Modules;
using LearningMpaAbp.Tasks.Dtos;

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
                mapper.CreateMap<CreateTaskInput, Tasks.Task>();
                mapper.CreateMap<TaskDto, UpdateTaskInput>();
                mapper.CreateMap<UpdateTaskInput, Tasks.Task>();
            });
        }

        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(Assembly.GetExecutingAssembly());
        }
    }
}
