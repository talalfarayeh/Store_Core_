using Microsoft.AspNetCore.Mvc;
using Store.Core.Application.DTOs;
using Store.Core.Application.Sarvice.ISarvice;

namespace Store_Core.Controllers
{
    public class OrderController : Controller
    {
        private readonly IOrderService _orderService;

        public OrderController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        public async Task<IActionResult> Index()
        {
            var orders = await _orderService.GetAllOrdersWithProductsAsync();
            return View(orders);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(OrderDto orderDto)
        {
            if (ModelState.IsValid)
            {
                await _orderService.AddOrderAsync(orderDto);
                return RedirectToAction("Index");
            }
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
