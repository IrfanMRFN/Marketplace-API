namespace Marketplace.Application.DTOs;

// Output DTO
public record ProductDto(
    int Id,
    string Name,
    string Description,
    decimal Price,
    int StockQuantity
);

// Input DTO
public record CreateProductDto(
    string Name,
    string Description,
    decimal Price,
    int StockQuantity
);
