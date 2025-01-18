

using Store.Infrastructure.Models;

namespace Store.Infrastructure.Repositories.IRepositories
{
    public interface IProductRepository
    {
       
        Task<List<Product>> GetAllProductsWithSuppliersAsync();
        Task<Product> GetProductsWithSuppliersByIdAsync(int productId);

    }
}
