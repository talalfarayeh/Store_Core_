using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Store.Core.Application.DTOs;
using Store.Core.Application.Sarvice.ISarvice;
using Store_Core.ViewModels;
using Store.Infrastructure.Models;
using System.Threading.Tasks;

namespace Store_Core.Controllers
{
    [Authorize]  
    public class OrderController : Controller
    {
        private readonly IOrderService _orderService;
        private readonly ILogger<OrderController> _logger;
        private readonly UserManager<ApplicationUser> _userManager;

        public OrderController(IOrderService orderService, ILogger<OrderController> logger, UserManager<ApplicationUser> userManager)
        {
            _orderService = orderService;
            _logger = logger;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index(string searchTerm = "", int pageNumber = 1, int pageSize = 10)
        {
            _logger.LogInformation("Fetching orders. SearchTerm: {SearchTerm}, Page: {PageNumber}, PageSize: {PageSize}", searchTerm, pageNumber, pageSize);

            try
            {
                var user = await _userManager.GetUserAsync(User);
                if (user == null)
                {
                    _logger.LogWarning("User not found.");
                    return RedirectToAction("Login", "Account");
                }

                PagedResponse<OrderDto> pagedResponse;

                if (User.IsInRole("Admin"))
                {
                    pagedResponse = await _orderService.GetAllOrdersWithProductsAsync(searchTerm, pageNumber, pageSize);
                }
                else
                {
                    pagedResponse = await _orderService.GetUserOrdersAsync(user.Id, pageNumber, pageSize);
                }

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

        [Authorize(Roles = "User")]
        public IActionResult Create()
        {
            _logger.LogInformation("Navigating to the Create Order page.");
            return View();
        }

        [HttpPost]
        [Authorize(Roles = "User")]
        public async Task<IActionResult> Create(OrderDto orderDto)
        {
            _logger.LogInformation("Attempting to create a new order.");

            try
            {
                var user = await _userManager.GetUserAsync(User);
                if (user == null)
                {
                    _logger.LogWarning("User not found.");
                    return RedirectToAction("Login", "Account");
                }

                orderDto.UserId = user.Id; 
                await _orderService.AddOrderAsync(orderDto, user.Id);

                _logger.LogInformation("Order created successfully.");
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while creating an order.");
                return View(orderDto);
            }
        }

        [Authorize(Roles = "Admin")]
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
        [Authorize(Roles = "Admin")]
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

        [Authorize(Roles = "Admin")]
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
        [Authorize(Roles = "Admin")]
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
