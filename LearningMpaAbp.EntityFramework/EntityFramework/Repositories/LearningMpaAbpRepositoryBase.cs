using Abp.Domain.Entities;
using Abp.EntityFramework;
using Abp.EntityFramework.Repositories;

namespace LearningMpaAbp.EntityFramework.Repositories
{
    public abstract class LearningMpaAbpRepositoryBase<TEntity, TPrimaryKey> : EfRepositoryBase<LearningMpaAbpDbContext, TEntity, TPrimaryKey>
        where TEntity : class, IEntity<TPrimaryKey>
    {
        protected LearningMpaAbpRepositoryBase(IDbContextProvider<LearningMpaAbpDbContext> dbContextProvider)
            : base(dbContextProvider)
        {

        }

        //add common methods for all repositories
    }

    public abstract class LearningMpaAbpRepositoryBase<TEntity> : LearningMpaAbpRepositoryBase<TEntity, int>
        where TEntity : class, IEntity<int>
    {
        protected LearningMpaAbpRepositoryBase(IDbContextProvider<LearningMpaAbpDbContext> dbContextProvider)
            : base(dbContextProvider)
        {

        }

        //do not add any method here, add to the class above (since this inherits it)
    }
}
