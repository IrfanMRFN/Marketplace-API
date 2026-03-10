namespace Marketplace.Application.DTOs;

public record CreateOrderDto(int ProductId, int Quantity);

public record OrderResponseDto(
    int OrderId,
    int ProductId,
    string ProductName,
    int Quantity,
    decimal TotalPrice,
    DateTime OrderDate
);