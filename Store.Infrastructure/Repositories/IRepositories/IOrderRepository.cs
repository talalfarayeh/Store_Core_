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
        Task<(List<Order>,int)> GetAllOrdersWithProductsAsync(string searchTerm, int pageNumber, int pageSize);
        Task<Order> GetOrdersWithProductsByIdAsync(int id);
    }
}
