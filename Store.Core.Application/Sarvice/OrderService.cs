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
    public class OrderService : IOrderService
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IGenericRepository<Order> _genericRepository;

        public OrderService(IOrderRepository orderRepository, IGenericRepository<Order> genericRepository)
        {
            _orderRepository = orderRepository;
            _genericRepository = genericRepository;
        }

        public async Task<List<OrderDto>> GetAllOrdersAsync()
        {
            var orders = await _orderRepository.GetAllOrdersWithProductsAsync();
            return orders.Adapt<List<OrderDto>>();
        }

        public async Task<OrderDto> AddOrderAsync(OrderDto orderDto)
        {
            var order = orderDto.Adapt<Order>();
            await _genericRepository.AddAsync(order);
            return order.Adapt<OrderDto>();
        }

        public async Task<OrderDto> GetOrderByIdAsync(int id)
        {
            var order = await _orderRepository.GetOrdersWithProductsByIdAsync(id);
            return order.Adapt<OrderDto>();
        }

        public async Task UpdateOrderAsync(OrderDto orderDto)
        {
            var order = orderDto.Adapt<Order>();
            await _genericRepository.UpdateAsync(order);
        }

        public async Task DeleteOrderAsync(int id)
        {
            await _genericRepository.DeleteAsync(await _genericRepository.GetByIdAsync(id));
        }

        public async Task<List<OrderDto>> GetAllOrdersWithProductsAsync()
        {
            var orders = await _orderRepository.GetAllOrdersWithProductsAsync();
            return orders.Adapt<List<OrderDto>>();
        }
    }
}
