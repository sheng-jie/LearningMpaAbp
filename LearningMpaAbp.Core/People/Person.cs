using System;
using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;

namespace LearningMpaAbp.People
{
    public class Person:Entity,IHasCreationTime
    {
        public string Name { get; set; }
        public DateTime CreationTime { get; set; }
    }
}