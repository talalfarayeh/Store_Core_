using Microsoft.AspNetCore.Mvc;
using Store.Core.Application.DTOs;
using Store.Core.Application.Sarvice.ISarvice;

namespace Store_Core.Controllers
{
    public class StockMovementController : Controller
    {

        private readonly IStockMovementService _stockMovementService;

        public StockMovementController(IStockMovementService stockMovementService)
        {
            _stockMovementService = stockMovementService;
        }

        public async Task<IActionResult> Index()
        {
            var stockMovements = await _stockMovementService.GetAllStockMovementsAsync();
            return View(stockMovements);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(StockMovementDto stockMovementDto)
        {
            
            
                await _stockMovementService.AddStockMovementAsync(stockMovementDto);
                
            
            return View(stockMovementDto);
        }
    }
}
