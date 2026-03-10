using Marketplace.Application.DTOs;

namespace Marketplace.Application.Interfaces;

public interface IProductService
{
    Task<IEnumerable<ProductDto>> GetAllProductsAsync();
    Task<ProductDto?> GetProductByIdAsync(int id);
    Task<ProductDto> CreateProductAsync(CreateProductDto request);
    Task<ProductDto?> UpdateProductAsync(int id, UpdateProductDto request);
    Task<bool> DeleteProductAsync(int id);
}
