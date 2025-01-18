using Mapster;
using Store.Core.Application.DTOs;
using Store.Core.Application.Sarvice.ISarvice;
using Store.Infrastructure.Models;
using Store.Infrastructure.Repositories.IRepositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Core.Application.Sarvice
{
    public class StockMovementSarvice : IStockMovementService
    {
        private readonly IStockMovementRepository _stockMovementRepository;
        IGenericRepository<Product> _productRepository;

        public StockMovementSarvice(IStockMovementRepository stockMovementRepository, IGenericRepository<Product> productRepository)
        {
            _stockMovementRepository = stockMovementRepository;
            _productRepository = productRepository;
        }
        public async Task<List<StockMovementDto>> GetAllStockMovementsAsync()
        {
            var stockMovements = await _stockMovementRepository.GetAllStockMovementsWithProductsAsync();
            return stockMovements.Adapt<List<StockMovementDto>>();
        }

        public async Task AddStockMovementAsync(StockMovementDto stockMovementDto)
        {
            var product = await _productRepository.GetByIdAsync(stockMovementDto.ProductID);
            if (product == null)
                throw new Exception("Product not found");
            if (stockMovementDto.MovementType == "Out")
            {
                if (product.QuantityInStock < stockMovementDto.Quantity)
                    throw new Exception("Insufficient stock");
                product.QuantityInStock -= stockMovementDto.Quantity;
            }
            else if (stockMovementDto.MovementType == "In")
            {
                product.QuantityInStock += stockMovementDto.Quantity;
            }
            else
            {
                throw new Exception("Invalid Movement Type. Must be 'إدخال' or 'خروج'");
            }

            
            await _productRepository.UpdateAsync(product);

            
            var stockMovement = stockMovementDto.Adapt<StockMovement>();
            await _stockMovementRepository.AddAsync(stockMovement);
        }
    }
}
