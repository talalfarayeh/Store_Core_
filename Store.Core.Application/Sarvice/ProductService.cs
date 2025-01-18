using Mapster;
using Microsoft.EntityFrameworkCore;
using Store.Core.Application.DTOs;
using Store.Core.Application.Sarvice.ISarvice;
using Store.Infrastructure.Models;
using Store.Infrastructure.Repositories.IRepositories;
 

namespace Store.Core.Application.Sarvice
{
    public class ProductService : IProductService
    {
        private readonly IGenericRepository<Product> _repository;
        private readonly IProductRepository _productRepository;
        public ProductService(IGenericRepository<Product> repository,IProductRepository productRepository)
        {
            _repository = repository;
            _productRepository = productRepository;
        }

        public async Task<IEnumerable<ProductDto>> GetAllProductsWithSuppliersAsync()
        {
            var products = await _productRepository.GetAllProductsWithSuppliersAsync();
            return products.Adapt<List<ProductDto>>();   
        }

        public async Task<ProductDto> GetProductWithSupplierByIdAsync(int id)
        {
            var products = await _productRepository.GetProductsWithSuppliersByIdAsync(id);
            return products.Adapt<ProductDto>();
        }

      
        public async Task AddProducAsync(ProductCreateUpdateDto productDto)
        {
            var product = productDto.Adapt<Product>();
            await  _repository.AddAsync(product);
        }

        public async Task DeleteProducAsync(int id)
        {
          await  _repository.DeleteAsync(await _productRepository.GetProductsWithSuppliersByIdAsync(id));
        }

      

        public async Task UpdateProducAsync(ProductCreateUpdateDto productDto)
        {
            var product = productDto.Adapt<Product>();
            await _repository.UpdateAsync(product);
        }

        

        

       public async Task<ProductCreateUpdateDto> GetProductWSupplierByIdAsync(int id)
        {
            var products = await _productRepository.GetProductsWithSuppliersByIdAsync(id);
            return products.Adapt<ProductCreateUpdateDto>();

        }

        
      

       
    }
}
