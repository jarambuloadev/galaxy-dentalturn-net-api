
using Galaxy.DentalTurn.DataAccess.Entities;
using System.Linq.Expressions;

namespace Galaxy.DentalTurn.Repositories.Interfaces
{
    public interface IBaseRepository<TEntity> where TEntity : BaseEntity
    {
        Task<TEntity> CreateAsync(TEntity entity);
        Task UpdateAsync();
        Task<TEntity?> GetByIdAsync(int id);

        Task<TEntity?> GetByPredicateAsync(Expression<Func<TEntity, bool>> predicate);
        Task<(ICollection<TResult> Result, int TotalCount)> ListAsync<TResult>(
            Expression<Func<TEntity, bool>> predicate,
            Expression<Func<TEntity, TResult>> selector,
            int pageNumber = 1, int pageSize = 10
        );

        Task DeleteAsync(int id);
    }
}
