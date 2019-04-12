namespace Database.Repositories
{
    public interface IRepository<TEntity> : IRepository<TEntity, int> where TEntity : class
    {
    }
}