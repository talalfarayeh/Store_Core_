using Mapster;
using Microsoft.AspNetCore.Mvc;
using Store.Core.Application.DTOs;
using Store.Core.Application.Sarvice.ISarvice;
using Store.Infrastructure.Models;

namespace Store_Core.Controllers
{
    public class SupplierController : Controller
    {
        private readonly ISupplierService _supplierService;
        public SupplierController(ISupplierService supplierService)
        {
            _supplierService = supplierService;

        }
        public async Task<IActionResult> index()
        {

            var suppliers = await _supplierService.GetSuppliersAllAsync();
            return View(suppliers);

        }
        public IActionResult Create()
        {
            return View();
           
        
        
        }
        [HttpPost]
    public async Task<IActionResult> Create(SupplierDto supplier)
    {
        
            await _supplierService.AddSupplierAsync(supplier);
            
        
        return View(supplier);
    }

    public async Task<IActionResult> Edit(int id)
    {
        var supplier = await _supplierService.GetSupplierByIdAsync(id);
        if (supplier == null)
        {
            return NotFound();
        }
        return View(supplier);
    }

    [HttpPost]
    public async Task<IActionResult> Edit(Supplier supplier)
    {
       
       
            await _supplierService.UpdateSupplierAsync(supplier);
            
        return View(supplier);
    }

    public async Task<IActionResult> Delete(int id)
    {
        var supplier = await _supplierService.GetSupplierByIdAsync(id);
        if (supplier == null)
        {
            return NotFound();
        }
        return View(supplier);
    }

    [HttpPost, ActionName("Delete")]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        await _supplierService.DeleteSupplierAsync(id);
        return RedirectToAction("Index");
    }
    }
}
