using Microsoft.EntityFrameworkCore;
using Store.Infrastructure.Data;
using Store.Infrastructure.Repositories.IRepositories;
 


namespace Store.Infrastructure.Repositories
{
    public class GenericRopository<T> : IGenericRepository<T> where T : class
    {
        private ApplicationDbContext _dbcontext;
        public GenericRopository(ApplicationDbContext dbcontext)
        {
           _dbcontext = dbcontext;

        }
        public async Task<T> GetByIdAsync(int id)
        {
            return await _dbcontext.FindAsync<T>(id);
        }

        public  IQueryable<T> GetTableAsTracking()
        {
            return  _dbcontext.Set<T>().AsTracking();
        }

        public IQueryable<T> GetTableNoTracking()
        {
            return _dbcontext.Set<T>().AsNoTracking().AsQueryable();
        }
        public async Task<T> AddAsync(T entity)
        {
           await _dbcontext.AddAsync<T>(entity);
            await _dbcontext.SaveChangesAsync();
            return entity;
        }

        public async Task AddRangeAsync(ICollection<T> entities)
        {
            await _dbcontext.Set<T>().AddRangeAsync(entities);
            await _dbcontext.SaveChangesAsync();
        }

        public async Task DeleteAsync(T entity)
        {
            _dbcontext.Remove<T>(entity);
            await _dbcontext.SaveChangesAsync();
        }

        

       

        public void RollBack()
        {
          _dbcontext.Database.RollbackTransaction();
        }

        public async Task SaveChangesAsync()
        {
            await _dbcontext.SaveChangesAsync();
        }

        public async Task UpdateAsync(T entity)
        {
            _dbcontext.Update<T>(entity);
            await _dbcontext.SaveChangesAsync();
        }

        public async Task UpdateRangeAsync(ICollection<T> entities)
        {
            _dbcontext.UpdateRange(entities);
            await _dbcontext.SaveChangesAsync();
        }
        public async Task<(List<T> Data, int TotalRecords)> GetPagedDataAsync(int pageNumber, int pageSize, IQueryable<T> filteredData)
        {
             
            var totalRecords = await filteredData.CountAsync();

           
            var data = await filteredData
                .Skip((pageNumber - 1) * pageSize)  
                .Take(pageSize)  
                .ToListAsync();  

            
            return (data, totalRecords);
        }


        public async Task AddRangeListAsync(IEnumerable<T> entities)
        {
            await _dbcontext.Set<T>().AddRangeAsync(entities);
            await _dbcontext.SaveChangesAsync();
        }
    }
}
