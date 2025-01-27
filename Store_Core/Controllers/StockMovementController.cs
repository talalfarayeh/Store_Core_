using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Store.Core.Application.DTOs;
using Store.Core.Application.Sarvice.ISarvice;

namespace Store_Core.Controllers
{
    [Authorize(Roles = "Admin")]
    public class StockMovementController : Controller
    {
        private readonly IStockMovementService _stockMovementService;
        private readonly ILogger<StockMovementController> _logger;

        public StockMovementController(IStockMovementService stockMovementService, ILogger<StockMovementController> logger)
        {
            _stockMovementService = stockMovementService;
            _logger = logger;
        }

        public async Task<IActionResult> Index()
        {
            _logger.LogInformation("Fetching all stock movements.");

            try
            {
                var stockMovements = await _stockMovementService.GetAllStockMovementsAsync();
                _logger.LogInformation("Successfully retrieved stock movements.");
                return View(stockMovements);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while retrieving stock movements.");
                return View("Error");
            }
        }

        public IActionResult Create()
        {
            _logger.LogInformation("Navigating to Create Stock Movement page.");
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(StockMovementDto stockMovementDto)
        {
            _logger.LogInformation("Attempting to create a new stock movement.");

          /*  if (!ModelState.IsValid)
            {
                _logger.LogWarning("Stock movement creation failed due to invalid data.");
                return View(stockMovementDto);
            }*/

            try
            {
                await _stockMovementService.AddStockMovementAsync(stockMovementDto);
                _logger.LogInformation("Stock movement created successfully.");
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while creating a stock movement.");
                return View(stockMovementDto);
            }
        }
    }
}
