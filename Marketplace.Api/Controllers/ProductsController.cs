using Marketplace.Application.DTOs;
using Marketplace.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Marketplace.Api.Controllers;

[ApiController] // Turns on automatic validation for DTOs
[Route("api/[controller]")] // Defines the base URL for every method in this class
public class ProductsController : ControllerBase
{
    private readonly IProductService _productService;

    public ProductsController(IProductService productService)
    {
        _productService = productService;
    }

    [HttpGet] // GET: api/products
    public async Task<ActionResult<IEnumerable<ProductDto>>> GetAllProducts()
    {
        var products = await _productService.GetAllProductsAsync();
        return Ok(products); // Returns a 200 OK with the JSON array
    }

    [HttpGet("{id}")] // GET: api/products/{id}
    public async Task<ActionResult<ProductDto>> GetProductById(int id)
    {
        var product = await _productService.GetProductByIdAsync(id);

        if (product == null)
            return NotFound(new { Message = $"Product with ID {id} not found."}); // 404

        return Ok(product); // 200
    }

    [HttpPost] // POST: api/products
    public async Task<ActionResult<ProductDto>> CreateProduct([FromBody] CreateProductDto request)
    {
        var newProduct = await _productService.CreateProductAsync(request);

        // Returns a 201 Created status, new product URL address and the new product itself
        return CreatedAtAction(nameof(GetProductById), new { id = newProduct.Id}, newProduct);
    }
}
