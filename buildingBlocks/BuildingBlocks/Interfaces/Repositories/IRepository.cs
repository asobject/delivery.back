using BuildingBlocks.Interfaces.Entities;
using BuildingBlocks.Models;
using System.Linq.Expressions;

namespace BuildingBlocks.Interfaces.Repositories
{
    public interface IRepository<T> where T : class, IEntity
    {
        Task<T?> GetByIdAsync(int id);
        Task AddAsync(T entity);
        Task UpdateAsync(T entity);
        Task DeleteAsync(T entity);
        Task<bool> ExistsAsync(Expression<Func<T, bool>> predicate);
        Task<PagedResultDTO<T>> GetPagedAsync(
        int pageNumber,
        int pageSize,
        Expression<Func<T, bool>>? predicate = null,
        params Func<IQueryable<T>, IQueryable<T>>[] includeFuncs);
        Task<IEnumerable<T>> FindAsync(
            Expression<Func<T, bool>> predicate,
            params Func<IQueryable<T>, IQueryable<T>>[] includeFuncs);
        Task<IEnumerable<T>> FindBySqlAsync(
         string sqlQuery,
         params object[] parameters);
        Task DeleteRangeAsync(IEnumerable<T> entities);
        Task<IEnumerable<T>> GetByIdsAsync(IEnumerable<int> ids);
    }
}
