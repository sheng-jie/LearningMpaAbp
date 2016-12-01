using AutoMapper;
using LearningMpaAbp.Tasks.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LearningMpaAbp.Tasks
{
    public class TaskDtoMapping : IDtoMapping
    {
        public void CreateMapping(IMapperConfigurationExpression mapperConfig)
        {
            mapperConfig.CreateMap<CreateTaskInput, Tasks.Task>();
            mapperConfig.CreateMap<TaskDto, UpdateTaskInput>();
            mapperConfig.CreateMap<UpdateTaskInput, Tasks.Task>();
            mapperConfig.CreateMap<TaskDto, Tasks.Task>();
            mapperConfig.CreateMap<Tasks.Task, TaskDto>();
        }
    }
}
