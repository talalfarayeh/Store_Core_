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
        private readonly ILogger<SupplierController> _logger;

        public SupplierController(ISupplierService supplierService, ILogger<SupplierController> logger)
        {
            _supplierService = supplierService;
            _logger = logger;
        }

        public async Task<IActionResult> Index()
        {
            _logger.LogInformation("Fetching all suppliers.");
            try
            {
                var suppliers = await _supplierService.GetSuppliersAllAsync();
                _logger.LogInformation("Successfully retrieved suppliers.");
                return View(suppliers);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while retrieving suppliers.");
                return View("Error");
            }
        }

        public IActionResult Create()
        {
            _logger.LogInformation("Navigating to Create Supplier page.");
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(SupplierDto supplierDto)
        {
            _logger.LogInformation("Attempting to create a new supplier.");

            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Supplier creation failed due to invalid data.");
                return View(supplierDto);
            }

            try
            {
                await _supplierService.AddSupplierAsync(supplierDto);
                _logger.LogInformation("Supplier created successfully.");
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while creating a supplier.");
                return View(supplierDto);
            }
        }

        public async Task<IActionResult> Edit(int id)
        {
            _logger.LogInformation("Fetching supplier for editing. ID: {SupplierId}", id);
            try
            {
                var supplier = await _supplierService.GetSupplierByIdAsync(id);
                if (supplier == null)
                {
                    _logger.LogWarning("Supplier with ID {SupplierId} not found.", id);
                    return NotFound();
                }
                return View(supplier);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while fetching supplier for edit.");
                return View("Error");
            }
        }

        [HttpPost]
        public async Task<IActionResult> Edit(SupplierDto supplierDto)
        {
            _logger.LogInformation("Attempting to update supplier. ID: {SupplierId}", supplierDto.SupplierID);

            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Supplier update failed due to invalid data.");
                return View(supplierDto);
            }

            try
            {
                await _supplierService.UpdateSupplierAsync(supplierDto);
                _logger.LogInformation("Supplier updated successfully.");
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while updating supplier.");
                return View(supplierDto);
            }
        }

        public async Task<IActionResult> Delete(int id)
        {
            _logger.LogInformation("Fetching supplier for deletion. ID: {SupplierId}", id);
            try
            {
                var supplier = await _supplierService.GetSupplierByIdAsync(id);
                if (supplier == null)
                {
                    _logger.LogWarning("Supplier with ID {SupplierId} not found.", id);
                    return NotFound();
                }
                return View(supplier);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while fetching supplier for deletion.");
                return View("Error");
            }
        }

        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            _logger.LogInformation("Attempting to delete supplier. ID: {SupplierId}", id);
            try
            {
                await _supplierService.DeleteSupplierAsync(id);
                _logger.LogInformation("Supplier deleted successfully.");
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while deleting supplier.");
                return View("Error");
            }
        }
    }
}
