using Galaxy.DentalTurn.DataAccess;
using Galaxy.DentalTurn.DataAccess.Context;
using Galaxy.DentalTurn.DataAccess.Entities;
using Galaxy.DentalTurn.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Galaxy.DentalTurn.Repositories.Implementations
{
    public class BaseRepository<TEntity> : IBaseRepository<TEntity> where TEntity : BaseEntity
    {
        protected readonly DentalTurnDbContext _context;

        public BaseRepository(DentalTurnDbContext context)
        {
            _context = context;
        }

        //metodo de creación
        public async Task<TEntity> CreateAsync(TEntity entity)
        {
            var result = _context.Set<TEntity>().Add(entity);
            await _context.SaveChangesAsync();
            return result.Entity;
        }

        //metodo de actualización (salva cambios ya aplicados sobre la entidad)
        public async Task UpdateAsync()
        {
            await _context.SaveChangesAsync();
        }

        //metodo de obtención por id (id entero)
        public async Task<TEntity?> GetByIdAsync(int id)
        {
            return await _context.Set<TEntity>().FirstOrDefaultAsync(p => p.Status && p.Id==id);
        }

        //metodo de obtención por predicado
        public async Task<TEntity?> GetByPredicateAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return await _context.Set<TEntity>().FirstOrDefaultAsync(predicate);
        }

        //metodo de listado con paginación
        public async Task<(ICollection<TResult> Result, int TotalCount)> ListAsync<TResult>
        (
            Expression<Func<TEntity, bool>> predicate,
            Expression<Func<TEntity, TResult>> selector,
            int pageNumber = 1, int pageSize = 10
        )
        {
            var result = await _context.Set<TEntity>()
                .Where(predicate)
                .OrderBy(p => p.Id)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .Select(selector)
                .ToListAsync();

            var totalCount = await _context.Set<TEntity>()
                .Where(predicate)
                .CountAsync();

            return (result, totalCount);
        }

        //Borrado de Datos
        public async Task DeleteAsync(int id)
        {
            await _context.Set<TEntity>()
                .Where(p => p.Id == id)
                .ExecuteUpdateAsync(p =>
                p.SetProperty(e => e.Status, false));               
            
        }

    }
}
