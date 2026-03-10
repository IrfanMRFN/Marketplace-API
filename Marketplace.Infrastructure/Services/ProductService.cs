using Marketplace.Application.DTOs;
using Marketplace.Application.Interfaces;
using Marketplace.Domain.Entities;
using Marketplace.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Marketplace.Infrastructure.Services;

public class ProductService : IProductService
{
    private readonly MarketplaceDbContext _db;

    public ProductService(MarketplaceDbContext db)
    {
        _db = db;
    }

    public async Task<IEnumerable<ProductDto>> GetAllProductsAsync()
    {
        var products = await _db.Products.ToListAsync();

        // Map the Domain Entities to the output DTOs
        return products.Select(p =>
            new ProductDto(p.Id, p.Name, p.Description, p.Price, p.StockQuantity));
    }

    public async Task<ProductDto?> GetProductByIdAsync(int id)
    {
        var product = await _db.Products.FindAsync(id);

        if (product == null) return null;

        return new ProductDto(product.Id, product.Name, product.Description, product.Price, product.StockQuantity);
    }

    public async Task<ProductDto> CreateProductAsync(CreateProductDto request)
    {
        // Map the input DTO to a new Domain Entity
        var newProduct = new Product
        {
            Name = request.Name,
            Description = request.Description,
            Price = request.Price,
            StockQuantity = request.StockQuantity
        };

        // Save it to SQL Server
        _db.Products.Add(newProduct);
        await _db.SaveChangesAsync();

        // Return mapped Output DTO
        return new ProductDto(newProduct.Id, newProduct.Name, newProduct.Description, newProduct.Price, newProduct.StockQuantity);
    }
}
