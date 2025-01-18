using Microsoft.AspNetCore.Mvc;
using Store.Core.Application.DTOs;
using Store.Core.Application.Sarvice.ISarvice;
using Store.Infrastructure.Models;

namespace Store_Core.Controllers
{
    public class ProductController : Controller
    {
        private readonly IProductService _productService;
        public ProductController(IProductService productService)
        {
            _productService = productService;
        }

        public async Task<IActionResult> Index()
        {
            var products = await _productService.GetAllProductsWithSuppliersAsync();
            return View(products); 
        }

        public IActionResult Create()
        {
            return View();



        }
        [HttpPost]
        public async Task<IActionResult> Create(ProductCreateUpdateDto productDto)
        {
            if (ModelState.IsValid)
            {
                await _productService.AddProducAsync(productDto);
                return RedirectToAction("Index");
            }
            return View(productDto);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var supplier = await _productService.GetProductWSupplierByIdAsync(id);
            if (supplier == null)
            {
                return NotFound();
            }
            return View(supplier);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(ProductCreateUpdateDto productDto)
        {
            if (ModelState.IsValid)
            {
                await _productService.UpdateProducAsync(productDto);
                return RedirectToAction("Index");
            }
            return View(productDto);
        }

        public async Task<IActionResult> Delete(int id)
        {
            var supplier = await _productService.GetProductWithSupplierByIdAsync(id);
            if (supplier == null)
            {
                return NotFound();
            }
            return View(supplier);
        }

        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _productService.DeleteProducAsync(id);
            return RedirectToAction("Index");
        }
    }
}

