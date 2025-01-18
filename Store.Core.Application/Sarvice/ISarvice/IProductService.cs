
using Store.Core.Application.DTOs;
using Store.Infrastructure.Models;

namespace Store.Core.Application.Sarvice.ISarvice
{
    public interface IProductService
    {
        Task<IEnumerable<ProductDto>> GetAllProductsWithSuppliersAsync();
        Task<ProductDto> GetProductWithSupplierByIdAsync(int id);
        Task<ProductCreateUpdateDto> GetProductWSupplierByIdAsync(int id);
        Task AddProducAsync(ProductCreateUpdateDto productDto);
        Task UpdateProducAsync(ProductCreateUpdateDto productDto);
        Task DeleteProducAsync(int id);
    }
}
