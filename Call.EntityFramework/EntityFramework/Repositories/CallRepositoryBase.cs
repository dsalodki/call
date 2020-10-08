using Abp.Domain.Entities;
using Abp.EntityFramework;
using Abp.EntityFramework.Repositories;

namespace Call.EntityFramework.Repositories
{
    public abstract class CallRepositoryBase<TEntity, TPrimaryKey> : EfRepositoryBase<CallDbContext, TEntity, TPrimaryKey>
        where TEntity : class, IEntity<TPrimaryKey>
    {
        protected CallRepositoryBase(IDbContextProvider<CallDbContext> dbContextProvider)
            : base(dbContextProvider)
        {

        }

        //add common methods for all repositories
    }

    public abstract class CallRepositoryBase<TEntity> : CallRepositoryBase<TEntity, int>
        where TEntity : class, IEntity<int>
    {
        protected CallRepositoryBase(IDbContextProvider<CallDbContext> dbContextProvider)
            : base(dbContextProvider)
        {

        }

        //do not add any method here, add to the class above (since this inherits it)
    }
}
