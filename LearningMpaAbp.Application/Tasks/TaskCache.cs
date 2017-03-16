using Abp.Dependency;
using Abp.Domain.Entities.Caching;
using Abp.Domain.Repositories;
using Abp.Runtime.Caching;
using LearningMpaAbp.Tasks.Dtos;

namespace LearningMpaAbp.Tasks
{
    public class TaskCache : EntityCache<Task, TaskCacheItem>, ITaskCache, ISingletonDependency
    {
        public TaskCache(ICacheManager cacheManager, IRepository<Task, int> repository, string cacheName = null) 
            : base(cacheManager, repository, cacheName)
        {
        }
    }
}