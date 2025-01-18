using Microsoft.EntityFrameworkCore.Storage;

namespace Store.Infrastructure.Repositories.IRepositories
{
    public interface IGenericRepository<T> where T : class
    {
         
        Task<T> GetByIdAsync(int id);
        Task SaveChangesAsync();
        void RollBack();
        IQueryable<T> GetTableNoTracking();
        IQueryable<T> GetTableAsTracking();
        Task<T> AddAsync(T entity);
        Task AddRangeAsync(ICollection<T> entities);
        Task UpdateAsync(T entity);
        Task UpdateRangeAsync(ICollection<T> entities);
        Task DeleteAsync(T entity);

    }
}
