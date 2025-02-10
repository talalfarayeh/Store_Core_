using Microsoft.AspNetCore.Mvc;
using Store.Core.Application.Sarvice.ISarvice;
using Store.Core.Application.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;

namespace Store_Core.Controllers.API
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin")]  
    public class ProductApiController : ControllerBase
    {
        private readonly IProductService _productService;
        private readonly ILogger<ProductApiController> _logger;

        public ProductApiController(IProductService productService, ILogger<ProductApiController> logger)
        {
            _productService = productService;
            _logger = logger;
        }

         
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProductDto>>> GetAllProducts(string searchTerm = "", int pageNumber = 1, int pageSize = 10)
        {
            try
            {
                _logger.LogInformation("Fetching products with pagination.");
                var pagedResponse = await _productService.GetPagedProductsAsync(searchTerm, pageNumber, pageSize);
                _logger.LogInformation("Products fetched successfully.");
                return Ok(pagedResponse);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching products.");
                
                return StatusCode(500, "Internal Server Error");
            }
        }

        
        [HttpGet("{id}")]
        public async Task<ActionResult<ProductDto>> GetProductById(int id)
        {
            try
            {
                _logger.LogInformation($"Fetching product with ID {id}.");
                var product = await _productService.GetProductWSupplierByIdAsync(id);
                if (product == null)
                {
                    _logger.LogWarning($"Product with ID {id} not found.");
                    return NotFound("Product not found");
                }
                return Ok(product);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error fetching product with ID {id}.");
                return StatusCode(500, "Internal Server Error");
            }
        }

        
        [HttpPost]
        public async Task<ActionResult> AddProduct([FromBody] ProductCreateUpdateDto productDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                _logger.LogInformation("Adding new product.");
                await _productService.AddMProductsAsync(new List<ProductCreateUpdateDto> { productDto });
                _logger.LogInformation("Product added successfully.");
                return CreatedAtAction(nameof(GetProductById), new { id = productDto.ProductID }, productDto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error adding product.");
                return StatusCode(500, "Internal Server Error");
            }
        }

       
        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateProduct(int id, [FromBody] ProductCreateUpdateDto productDto)
        {
            if (id != productDto.ProductID)
                return BadRequest("Product ID mismatch");

            try
            {
                _logger.LogInformation($"Updating product with ID {id}.");
                await _productService.UpdateProducAsync(productDto);
                _logger.LogInformation($"Product with ID {id} updated successfully.");
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error updating product with ID {id}.");
                return StatusCode(500, "Internal Server Error");
            }
        }

      
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteProduct(int id)
        {
            try
            {
                _logger.LogInformation($"Deleting product with ID {id}.");
                await _productService.DeleteProducAsync(id);
                _logger.LogInformation($"Product with ID {id} deleted successfully.");
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error deleting product with ID {id}.");
                return StatusCode(500, "Internal Server Error");
            }
        }
    }
}
