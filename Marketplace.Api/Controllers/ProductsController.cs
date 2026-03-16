using Marketplace.Application.DTOs;
using Marketplace.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;

namespace Marketplace.Api.Controllers;

[ApiController] // Turns on automatic validation for DTOs
[Route("api/[controller]")] // Defines the base URL for every method in this class
[EnableRateLimiting("StrictPolicy")]
public class ProductsController : ApiControllerBase
{
    private readonly IProductService _productService;

    public ProductsController(IProductService productService)
    {
        _productService = productService;
    }

    [HttpGet] // GET: api/products
    public async Task<ActionResult<IEnumerable<ProductDto>>> GetAllProducts()
    {
        var result = await _productService.GetAllProductsAsync();
        return result.IsSuccess ? Ok(result.Value) : HandleFailure(result);
    }

    [HttpGet("{id}")] // GET: api/products/{id}
    public async Task<ActionResult<ProductDto>> GetProductById(int id)
    {
        var result = await _productService.GetProductByIdAsync(id);
        return result.IsSuccess ? Ok(result.Value) : HandleFailure(result);
    }

    [Authorize]
    [HttpPost] // POST: api/products (SECURED)
    public async Task<ActionResult<ProductDto>> CreateProduct([FromBody] CreateProductDto request)
    {
        var result = await _productService.CreateProductAsync(request);

        return result.IsSuccess
            ? CreatedAtAction(nameof(GetProductById), new { id = result.Value!.Id}, result.Value)
            : HandleFailure(result);
    }

    [Authorize]
    [HttpPut("{id}")] // PUT: api/products/{id} (SECURED)
    public async Task<ActionResult<ProductDto>> UpdateProduct(int id, [FromBody] UpdateProductDto request)
    {
        var result = await _productService.UpdateProductAsync(id, request);
        return result.IsSuccess ? Ok(result.Value) : HandleFailure(result);
    }

    [Authorize]
    [HttpDelete("{id}")] // DELETE: api/products/{id} (SECURED)
    public async Task<IActionResult> DeleteProduct(int id)
    {
        var result = await _productService.DeleteProductAsync(id);

        return result.IsSuccess ? NoContent() : HandleFailure(result); // 204 No Content, Standard successful delete response
    } 
}
