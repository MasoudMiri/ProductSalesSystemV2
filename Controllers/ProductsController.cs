using Microsoft.AspNetCore.Mvc;
using MyProject.Application.DTOs;
using MyProject.Application.Services;
using Serilog;

namespace Product_sales_system.Controllers  // ← این رو درست کن
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IProductService _productService;

        public ProductsController(IProductService productService)
        {
            _productService = productService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            Log.Information("Getting all products");
            var products = await _productService.GetAllAsync();
            return Ok(products);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            Log.Information("Getting product with id: {Id}", id);
            var product = await _productService.GetByIdAsync(id);
            if (product == null)
            {
                Log.Warning("Product with id {Id} not found", id);
                return NotFound();
            }
            return Ok(product);
        }

        [HttpPost]
        public async Task<IActionResult> Add([FromBody] ProductDto productDto)
        {
            Log.Information("Adding new product: {@Product}", productDto);
            await _productService.AddAsync(productDto);
            return CreatedAtAction(nameof(GetById), new { id = productDto.Id }, productDto);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] ProductDto productDto)
        {
            if (id != productDto.Id)
            {
                Log.Warning("ID mismatch: {Id} vs {DtoId}", id, productDto.Id);
                return BadRequest("ID mismatch");
            }

            Log.Information("Updating product: {@Product}", productDto);
            await _productService.UpdateAsync(productDto);
            return NoContent();
        }

        [HttpPatch("{id}/toggle-active")]
        public async Task<IActionResult> ToggleActive(int id)
        {
            Log.Information("Toggling active status for product id: {Id}", id);
            await _productService.ToggleActiveAsync(id);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            Log.Information("Deleting product with id: {Id}", id);
            await _productService.DeleteAsync(id);
            return NoContent();
        }
    }
}