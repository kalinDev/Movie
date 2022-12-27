using System.Linq.Expressions;

namespace MovieApi.Domain.Interfaces;

public interface IRepository<TEntity>
{
    Task<IEnumerable<TEntity>> SearchAsync(Expression<Func<TEntity, bool>> predicate);
    Task<TEntity> FindByIdAsync(int id);
    Task<List<TEntity>> FindAsync();
    void Add(TEntity entity);
    void Update(TEntity entity);
    void Remove(int id);
    Task<bool> AnyAsync(int id);
    Task<int> SaveChangesAsync();
}