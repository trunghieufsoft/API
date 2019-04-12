using Microsoft.EntityFrameworkCore.Design;
using Database.EntityFrameworkCore;

namespace Database.Repositories
{
    public class Repository<TEntity> : Repository<TEntity, int>, IRepository<TEntity> where TEntity : class
    {
        public Repository(IDesignTimeDbContextFactory<APIDbContext> dbContextFactory) : base(dbContextFactory)
        {
        }
    }
}