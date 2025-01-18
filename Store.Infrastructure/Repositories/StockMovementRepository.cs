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
    public class StockMovementRepository : GenericRopository<StockMovement>, IStockMovementRepository
    {
        private readonly ApplicationDbContext _context;

        public StockMovementRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }
        public Task<List<StockMovement>> GetAllStockMovementsWithProductsAsync()
        {
            return _context.StockMovements.Include(x => x.Product).ToListAsync();
        }

        public Task<StockMovement> GetStockMovementsWithProductsByIdAsync(int id)
        {
            return _context.StockMovements.Include(x => x.Product).FirstOrDefaultAsync(x=>x.StockMovementID==id);
        }
    }
}
