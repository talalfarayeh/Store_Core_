using Mapster;
using Microsoft.AspNetCore.Mvc;
using Store.Core.Application.DTOs;
 
using Store.Core.Application.Sarvice.ISarvice;
using Store.Infrastructure.Models;
 

using Store_Core.ViewModels;
namespace Store_Core.Controllers
{
    public class ProductController : Controller
    {
        private readonly IProductService _productService;
        private readonly ILogger<ProductController> _logger;
        public ProductController(IProductService productService, ILogger<ProductController> logger)
        {
            _productService = productService;
            _logger = logger;
        }

         
        public async Task<IActionResult> Index(string searchTerm = "", int pageNumber = 1, int pageSize = 10)
        {
            try
            {
                _logger.LogInformation("Starting to fetch products with pagination."); 

                var pagedResponse = await _productService.GetPagedProductsAsync(searchTerm, pageNumber, pageSize);
                _logger.LogInformation("Successfully fetched products.");  
                var viewModel = new ProductsViewModel
                {
                    Products = pagedResponse.Data.ToList(),
                    TotalRecords = pagedResponse.TotalRecords,
                    CurrentPage = pageNumber,
                    PageSize = pageSize,
                    SearchTerm = searchTerm
                };

                return View(viewModel);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while fetching products.");
                return StatusCode(500, "Internal Server Error");
            }
        }



        public IActionResult Create()
        {
            _logger.LogInformation("Rendering Create view for adding multiple products.");
            var viewModel = new AddMultipleProductsViewModel
            {
                Products = new List<ProductCreateUpdateDto>
            {
                new ProductCreateUpdateDto(),
                new ProductCreateUpdateDto(),
                new ProductCreateUpdateDto(),
                new ProductCreateUpdateDto(), 
                new ProductCreateUpdateDto(),
                new ProductCreateUpdateDto()
            }
            };
            return View(viewModel);
        }

      
        [HttpPost]
        public async Task<IActionResult> Create(AddMultipleProductsViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("ModelState is invalid while adding multiple products.");
                return View(viewModel);
            }

            try
            {
                _logger.LogInformation("Adding multiple products.");
                var products = viewModel.Products.Adapt<List<ProductCreateUpdateDto>>();
                await _productService.AddMProductsAsync(products);
                _logger.LogInformation("Products added successfully.");
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while adding products.");
                return StatusCode(500, "Internal Server Error");
            }
        }

        
        public async Task<IActionResult> Edit(int id)
        {
            try
            {
                _logger.LogInformation($"Fetching product with ID {id} for editing.");
                var supplier = await _productService.GetProductWSupplierByIdAsync(id);
                if (supplier == null)
                {
                    _logger.LogWarning($"Product with ID {id} not found.");
                    return NotFound();
                }
                return View(supplier);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occurred while fetching product with ID {id} for editing.");
                return StatusCode(500, "Internal Server Error");
            }
        }

         
        [HttpPost]
        public async Task<IActionResult> Edit(ProductCreateUpdateDto productDto)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("ModelState is invalid while updating a product.");
                return View(productDto);
            }

            try
            {
                _logger.LogInformation($"Updating product with ID {productDto.ProductID}.");
                await _productService.UpdateProducAsync(productDto);
                _logger.LogInformation($"Product with ID {productDto.ProductID} updated successfully.");
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occurred while updating product with ID {productDto.ProductID}.");
                return StatusCode(500, "Internal Server Error");
            }
        }

      
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                _logger.LogInformation($"Fetching product with ID {id} for deletion.");
                var supplier = await _productService.GetProductWithSupplierByIdAsync(id);
                if (supplier == null)
                {
                    _logger.LogWarning($"Product with ID {id} not found for deletion.");
                    return NotFound();
                }
                return View(supplier);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occurred while fetching product with ID {id} for deletion.");
                return StatusCode(500, "Internal Server Error");
            }
        }

      
        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                _logger.LogInformation($"Deleting product with ID {id}.");
                await _productService.DeleteProducAsync(id);
                _logger.LogInformation($"Product with ID {id} deleted successfully.");
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occurred while deleting product with ID {id}.");
                return StatusCode(500, "Internal Server Error");
            }
        }
    }
}

