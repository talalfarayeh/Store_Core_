using Store.Core.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Core.Application.Sarvice.ISarvice
{
    public interface IOrderService
    {

        Task<PagedResponse<OrderDto>> GetAllOrdersWithProductsAsync(string searchTerm, int pageNumber, int pageSize);
        Task<OrderDto> GetOrderByIdAsync(int id);
        Task AddOrderAsync(OrderDto orderDto, string userId);
        Task UpdateOrderAsync(OrderDto orderDto);
        Task DeleteOrderAsync(int id);
        Task<PagedResponse<OrderDto>> GetUserOrdersAsync(string userId, int pageNumber, int pageSize);
    }
}
