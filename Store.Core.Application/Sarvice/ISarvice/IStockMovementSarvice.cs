using Store.Core.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Core.Application.Sarvice.ISarvice
{
    public interface IStockMovementService
    {
        Task<List<StockMovementDto>> GetAllStockMovementsAsync();
        Task AddStockMovementAsync(StockMovementDto stockMovementDto);

    }
}
