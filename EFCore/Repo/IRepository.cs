using System.Linq.Expressions;

namespace EFCore.Repo;

public interface IRepository<TEntity> where TEntity:class
{
    Task<IEnumerable<TEntity>> GetAllAsync();

    Task<TEntity> GetByIdAsync(int id);

    Task AddAsync(TEntity entity);

    Task UpdateAsync(TEntity entity, int id);

    Task DeleteAsync(int id);

    Task<TEntity> GetByAsync(Expression<Func<TEntity, bool>> predicate);

    Task<IEnumerable<TEntity>> GetMultipleByAsync(Expression<Func<TEntity, bool>> predicate);

}