using Microsoft.AspNetCore.Mvc;
using Store.Core.Application.DTOs;
using Store.Core.Application.Sarvice.ISarvice;
using Store_Core.ViewModels;

namespace Store_Core.Controllers
{
    public class OrderController : Controller
    {
        private readonly IOrderService _orderService;

        public OrderController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        public async Task<IActionResult> Index(string searchTerm = "", int pageNumber = 1, int pageSize = 10)
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

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(OrderDto orderDto)
        {
            
            
                await _orderService.AddOrderAsync(orderDto);
                
            
            return View(orderDto);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var order = await _orderService.GetOrderByIdAsync(id);
            if (order == null)
                return NotFound();
            return View(order);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(OrderDto orderDto)
        {
            if (ModelState.IsValid)
            {
                await _orderService.UpdateOrderAsync(orderDto);
                return RedirectToAction("Index");
            }
            return View(orderDto);
        }

        public async Task<IActionResult> Delete(int id)
        {
            var order = await _orderService.GetOrderByIdAsync(id);
            if (order == null)
                return NotFound();
            return View(order);
        }

        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _orderService.DeleteOrderAsync(id);
            return RedirectToAction("Index");
        }
    }
}
