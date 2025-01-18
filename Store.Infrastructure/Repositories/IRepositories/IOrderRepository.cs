using Store.Infrastructure.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Infrastructure.Repositories.IRepositories
{
    public interface IOrderRepository
    {
        Task<List<Order>> GetAllOrdersWithProductsAsync();
        Task<Order> GetOrdersWithProductsByIdAsync(int id);
    }
}
