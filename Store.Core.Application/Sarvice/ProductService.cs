using Mapster;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Store.Core.Application.DTOs;
 
using Store.Core.Application.Sarvice.ISarvice;
using Store.Infrastructure.Models;
using Store.Infrastructure.Repositories.IRepositories;
 

namespace Store.Core.Application.Sarvice
{
    public class ProductService : IProductService
    {
        private readonly ILogger<ProductService> _logger;
        private readonly IGenericRepository<Product> _repository;
        private readonly IProductRepository _productRepository;
        public ProductService(IGenericRepository<Product> repository, IProductRepository productRepository, ILogger<ProductService> logger)
        {
            _repository = repository;
            _productRepository = productRepository;
            _logger = logger;
        }

        public async Task<IEnumerable<ProductDto>> GetAllProductsWithSuppliersAsync()
        {
            try
            {
                _logger.LogInformation("Fetching all products with their suppliers.");
                var products = await _productRepository.GetAllProductsWithSuppliersAsync();
                _logger.LogInformation("Successfully fetched all products with suppliers.");
                return products.Adapt<List<ProductDto>>();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while fetching all products with suppliers.");
                throw;
            }
        }

        public async Task<ProductDto> GetProductWithSupplierByIdAsync(int id)
        {
            try
            {
                _logger.LogInformation($"Fetching product with ID {id} and its supplier.");
                var product = await _productRepository.GetProductsWithSuppliersByIdAsync(id);
                if (product == null)
                {
                    _logger.LogWarning($"Product with ID {id} not found.");
                    return null;
                }
                _logger.LogInformation($"Successfully fetched product with ID {id} and its supplier.");
                return product.Adapt<ProductDto>();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occurred while fetching product with ID {id}.");
                throw;
            }
        }

        public async Task AddProducAsync(ProductCreateUpdateDto productDto)
        {
            try
            {
                _logger.LogInformation("Adding a new product.");
                var product = productDto.Adapt<Product>();
                await _repository.AddAsync(product);
                _logger.LogInformation($"Product added successfully: {product.ProductID}.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while adding a new product.");
                throw;
            }
        }

        public async Task DeleteProducAsync(int id)
        {
            try
            {
                _logger.LogInformation($"Deleting product with ID {id}.");
                var product = await _productRepository.GetProductsWithSuppliersByIdAsync(id);
                if (product == null)
                {
                    _logger.LogWarning($"Product with ID {id} not found for deletion.");
                    return;
                }
                await _repository.DeleteAsync(product);
                _logger.LogInformation($"Product with ID {id} deleted successfully.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occurred while deleting product with ID {id}.");
                throw;
            }
        }

        public async Task UpdateProducAsync(ProductCreateUpdateDto productDto)
        {
            try
            {
                _logger.LogInformation($"Updating product with ID {productDto.ProductID}.");
                var product = productDto.Adapt<Product>();
                await _repository.UpdateAsync(product);
                _logger.LogInformation($"Product with ID {productDto.ProductID} updated successfully.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occurred while updating product with ID {productDto.ProductID}.");
                throw;
            }
        }

        public async Task<ProductCreateUpdateDto> GetProductWSupplierByIdAsync(int id)
        {
            try
            {
                _logger.LogInformation($"Fetching product with ID {id} for editing.");
                var product = await _productRepository.GetProductsWithSuppliersByIdAsync(id);
                if (product == null)
                {
                    _logger.LogWarning($"Product with ID {id} not found.");
                    return null;
                }
                _logger.LogInformation($"Successfully fetched product with ID {id} for editing.");
                return product.Adapt<ProductCreateUpdateDto>();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occurred while fetching product with ID {id} for editing.");
                throw;
            }
        }

        public async Task AddMProductsAsync(List<ProductCreateUpdateDto> products)
        {
            try
            {
                _logger.LogInformation("Adding multiple products.");
                var entities = products.Adapt<IEnumerable<Product>>();
                await _repository.AddRangeListAsync(entities.ToList());
                _logger.LogInformation("Multiple products added successfully.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while adding multiple products.");
                throw;
            }
        }

        public async Task<PagedResponse<ProductDto>> GetPagedProductsAsync(string searchTerm, int pageNumber, int pageSize)
        {
            try
            {
                var (products, totalRecords) = await _productRepository.GetPagedAndFilteredProductsAsync(searchTerm, pageNumber, pageSize);

                
                var productDtos = products.Adapt<List<ProductDto>>();

                return new PagedResponse<ProductDto>(productDtos, totalRecords, pageNumber, pageSize);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while fetching products.");
                throw;
            }

        }
    }
}
