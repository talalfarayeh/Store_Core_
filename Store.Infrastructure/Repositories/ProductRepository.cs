using Microsoft.EntityFrameworkCore;
using Store.Infrastructure.Data;
using Store.Infrastructure.Models;
using Store.Infrastructure.Repositories.IRepositories;
 

namespace Store.Infrastructure.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly ApplicationDbContext _context;
        public ProductRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<List<Product>> GetAllProductsWithSuppliersAsync()
        {
          return  await _context.Products.Include(x => x.Supplier).ToListAsync();

        }

        public async Task<Product> GetProductsWithSuppliersByIdAsync(int id)
        {
            return await _context.Products
                .Include(p => p.Supplier)
                .FirstOrDefaultAsync(p => p.ProductID == id);
        }
    }
}
