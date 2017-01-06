using AutoMapper;
using LearningMpaAbp.Tasks.Dtos;

namespace LearningMpaAbp.Tasks
{
    public class TaskDtoMapping : IDtoMapping
    {
        public void CreateMapping(IMapperConfigurationExpression mapperConfig)
        {
            //定义单向映射
            mapperConfig.CreateMap<CreateTaskInput, Task>();
            mapperConfig.CreateMap<UpdateTaskInput, Task>();
            mapperConfig.CreateMap<TaskDto, UpdateTaskInput>();

            //自定义映射
            var taskDtoMapper = mapperConfig.CreateMap<Task, TaskDto>();
            taskDtoMapper.ForMember(dto => dto.AssignedPersonName, map => map.MapFrom(m => m.AssignedPerson.FullName));
        }
    }
}