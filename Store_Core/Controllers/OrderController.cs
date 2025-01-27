using Microsoft.AspNetCore.Mvc;
using Store.Core.Application.DTOs;
using Store.Core.Application.Sarvice.ISarvice;
using Store_Core.ViewModels;

namespace Store_Core.Controllers
{
    public class OrderController : Controller
    {
        private readonly IOrderService _orderService;
        private readonly ILogger<OrderController> _logger;

        public OrderController(IOrderService orderService, ILogger<OrderController> logger)
        {
            _orderService = orderService;
            _logger = logger;
        }

        public async Task<IActionResult> Index(string searchTerm = "", int pageNumber = 1, int pageSize = 10)
        {
            _logger.LogInformation("Fetching orders. SearchTerm: {SearchTerm}, Page: {PageNumber}, PageSize: {PageSize}", searchTerm, pageNumber, pageSize);

            try
            {
                var pagedResponse = await _orderService.GetAllOrdersWithProductsAsync(searchTerm, pageNumber, pageSize);
                var ViewModel = new OrderViewModel
                {
                    Orders = pagedResponse.Data.ToList(),
                    TotalRecords = pagedResponse.TotalRecords,
                    CurrentPage = pageNumber,
                    PageSize = pageSize,
                    SearchTerm = searchTerm
                };

                return View(ViewModel);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while retrieving orders.");
                return View("Error");
            }
        }

        public IActionResult Create()
        {
            _logger.LogInformation("Navigating to the Create Order page.");
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(OrderDto orderDto)
        {
            _logger.LogInformation("Attempting to create a new order.");

            /*if (!ModelState.IsValid)
            {
                _logger.LogWarning("Order creation failed due to invalid data.");
                return View(orderDto);
            }*/

            try
            {
                await _orderService.AddOrderAsync(orderDto);
                _logger.LogInformation("Order created successfully.");
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while creating an order.");
                return View(orderDto);
            }
        }

        public async Task<IActionResult> Edit(int id)
        {
            _logger.LogInformation("Fetching order for editing. OrderId: {OrderId}", id);

            try
            {
                var order = await _orderService.GetOrderByIdAsync(id);
                if (order == null)
                {
                    _logger.LogWarning("Order not found. OrderId: {OrderId}", id);
                    return NotFound();
                }

                return View(order);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while retrieving order. OrderId: {OrderId}", id);
                return View("Error");
            }
        }

        [HttpPost]
        public async Task<IActionResult> Edit(OrderDto orderDto)
        {
            _logger.LogInformation("Attempting to edit order. OrderId: {OrderId}", orderDto.OrderID);

            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Order update failed due to invalid data.");
                return View(orderDto);
            }

            try
            {
                await _orderService.UpdateOrderAsync(orderDto);
                _logger.LogInformation("Order updated successfully. OrderId: {OrderId}", orderDto.OrderID);
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while updating order. OrderId: {OrderId}", orderDto.OrderID);
                return View(orderDto);
            }
        }

        public async Task<IActionResult> Delete(int id)
        {
            _logger.LogInformation("Fetching order for deletion. OrderId: {OrderId}", id);

            try
            {
                var order = await _orderService.GetOrderByIdAsync(id);
                if (order == null)
                {
                    _logger.LogWarning("Order not found. OrderId: {OrderId}", id);
                    return NotFound();
                }

                return View(order);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while retrieving order for deletion. OrderId: {OrderId}", id);
                return View("Error");
            }
        }

        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            _logger.LogInformation("Attempting to delete order. OrderId: {OrderId}", id);

            try
            {
                await _orderService.DeleteOrderAsync(id);
                _logger.LogInformation("Order deleted successfully. OrderId: {OrderId}", id);
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while deleting order. OrderId: {OrderId}", id);
                return View("Error");
            }
        }
    }
}
