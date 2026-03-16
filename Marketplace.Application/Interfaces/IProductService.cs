using Marketplace.Application.Common;
using Marketplace.Application.DTOs;

namespace Marketplace.Application.Interfaces;

public interface IProductService
{
    Task<Result<IEnumerable<ProductDto>>> GetAllProductsAsync();
    Task<Result<ProductDto>> GetProductByIdAsync(int id);
    Task<Result<ProductDto>> CreateProductAsync(CreateProductDto request);
    Task<Result<ProductDto>> UpdateProductAsync(int id, UpdateProductDto request);
    Task<Result<bool>> DeleteProductAsync(int id);
}
