using Microsoft.EntityFrameworkCore;
using Store.Infrastructure.Data;
using Store.Infrastructure.Models;
using Store.Infrastructure.Repositories.IRepositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Infrastructure.Repositories
{
    public class OrderRepository : IOrderRepository
    {
        private readonly ApplicationDbContext _context;
        public OrderRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<List<Order>> GetAllOrdersWithProductsAsync()
        {
            return await _context.Orders.Include(x=>x.Product).ToListAsync();
        }

        public Task<Order> GetOrdersWithProductsByIdAsync(int id)
        {
            return _context.Orders.Include(x => x.Product).FirstOrDefaultAsync(x => x.OrderID == id);
        }
    }
}
