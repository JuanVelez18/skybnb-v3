namespace repository.Interfaces
{
    public interface IBaseRepository<TEntity, TKey> where TEntity: class
    {
        Task<TEntity?> GetByIdAsync(TKey id);
        Task<List<TEntity>> GetAllAsync();
        Task AddAsync(TEntity entity);
        void Update(TEntity entity);
        void Delete(TEntity entity);
    }
}
