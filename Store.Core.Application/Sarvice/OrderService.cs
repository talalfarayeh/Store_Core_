using Mapster;
using Microsoft.Extensions.Logging;
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
    public class OrderService : IOrderService
    {
        private readonly ILogger<OrderService> _logger;
        private readonly IOrderRepository _orderRepository;
        private readonly IGenericRepository<Product> _productRepository;
        private readonly IGenericRepository<Order> _genericRepository;

        public OrderService(ILogger<OrderService> logger, IGenericRepository<Product> productRepository, IOrderRepository orderRepository, IGenericRepository<Order> genericRepository)
        {
            _orderRepository = orderRepository;
            _genericRepository = genericRepository;
            _productRepository = productRepository;
            _logger = logger;
        }

       

        public async Task AddOrderAsync(OrderDto orderDto, string userId)
        {
            try
            {

                _logger.LogInformation("Fetching all.");
                var product = await _productRepository.GetByIdAsync(orderDto.ProductID);
                if (product == null)
                    throw new Exception("Product not found");

                if (product.QuantityInStock < orderDto.Quantity)
                    throw new Exception("Insufficient stock");

                product.QuantityInStock -= orderDto.Quantity;
                await _productRepository.UpdateAsync(product);


                var order = orderDto.Adapt<Order>();
                order.UserId = userId;
                await _genericRepository.AddAsync(order);
            }
            catch (Exception)
            {
                throw;

            }
        }
        public async Task<PagedResponse<OrderDto>> GetUserOrdersAsync(string userId, int pageNumber, int pageSize)
        {
            var (orders, totalRecords) = await _orderRepository.GetUserOrdersAsync(userId, pageNumber, pageSize);
            var orderDtos = orders.Adapt<List<OrderDto>>();
            return new PagedResponse<OrderDto>(orderDtos, totalRecords, pageNumber, pageSize);
        }
        public async Task<OrderDto> GetOrderByIdAsync(int id)
        {
            var order = await _orderRepository.GetOrdersWithProductsByIdAsync(id);
            return order.Adapt<OrderDto>();
        }

        public async Task UpdateOrderAsync(OrderDto orderDto)
        {
            var existingOrder = await _orderRepository.GetOrdersWithProductsByIdAsync(orderDto.OrderID);
            if (existingOrder == null)
                throw new Exception("Order not found");

            var product = await _productRepository.GetByIdAsync(orderDto.ProductID);
            if (product == null)
                throw new Exception("Product not found");
            product.QuantityInStock += existingOrder.Quantity;

            if (product.QuantityInStock < orderDto.Quantity)
                throw new Exception("Insufficient stock");

            product.QuantityInStock -= orderDto.Quantity;
            await _productRepository.UpdateAsync(product);

            existingOrder = orderDto.Adapt(existingOrder);
            await _genericRepository.UpdateAsync(existingOrder);
        }

        public async Task DeleteOrderAsync(int id)
        {
            var existingOrder = await _orderRepository.GetOrdersWithProductsByIdAsync(id);
            if (existingOrder == null)
                throw new Exception("Order not found");

            var product = await _productRepository.GetByIdAsync(existingOrder.ProductID);
            if (product != null)
            {
                
                product.QuantityInStock += existingOrder.Quantity;
                await _productRepository.UpdateAsync(product);
            }

            
            await _genericRepository.DeleteAsync(existingOrder);
        }

        public async Task<PagedResponse<OrderDto>> GetAllOrdersWithProductsAsync(string searchTerm, int pageNumber, int pageSize)
        {
             var (orders, totalRecords) = await _orderRepository.GetAllOrdersWithProductsAsync( searchTerm,  pageNumber,  pageSize);

            var orderDtos = orders.Adapt<List<OrderDto>>();

            return new PagedResponse<OrderDto> (orderDtos,  totalRecords, pageNumber, pageSize );
        }

    }
}
