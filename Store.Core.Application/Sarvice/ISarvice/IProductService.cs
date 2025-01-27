
using Store.Core.Application.DTOs;
using Store.Infrastructure.Models;

namespace Store.Core.Application.Sarvice.ISarvice
{
    public interface IProductService
    {
       /* Task<IEnumerable<ProductDto>> GetProductsAsync(string searchTerm);*/
        Task AddMProductsAsync(List<ProductCreateUpdateDto> products);
        Task<IEnumerable<ProductDto>> GetAllProductsWithSuppliersAsync();
        Task<ProductDto> GetProductWithSupplierByIdAsync(int id);
        Task<ProductCreateUpdateDto> GetProductWSupplierByIdAsync(int id);
        Task<PagedResponse<ProductDto>> GetPagedProductsAsync(string searchTerm, int pageNumber, int pageSize);

        /*        Task<PagedResponse<ProductDto>> GetPagedProductsAsync(PaginationParams paginationParams, string searchTerm);
        */
        Task AddProducAsync(ProductCreateUpdateDto productDto);
        Task UpdateProducAsync(ProductCreateUpdateDto productDto);
        Task DeleteProducAsync(int id);
    }
}
