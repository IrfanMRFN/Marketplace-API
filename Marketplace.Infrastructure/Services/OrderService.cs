using Marketplace.Application.DTOs;
using Marketplace.Application.Interfaces;
using Marketplace.Domain.Entities;
using Marketplace.Infrastructure.Data;

namespace Marketplace.Infrastructure.Services;

public class OrderService : IOrderService
{
    private readonly MarketplaceDbContext _db;

    public OrderService(MarketplaceDbContext db)
    {
        _db = db;
    }

    public async Task<OrderResponseDto> CreateOrderAsync(int userId, CreateOrderDto request)
    {
        var product = await _db.Products.FindAsync(request.ProductId);
        if (product == null)
            throw new KeyNotFoundException($"Product with ID {request.ProductId} was not found.");

        if (product.StockQuantity < request.Quantity)
            throw new InvalidOperationException($"Not enough stock. Only {product.StockQuantity} available.");

        // Calculate total and reduce stocks
        var totalPrice = product.Price * request.Quantity;
        product.StockQuantity -= request.Quantity;

        var order = new Order
        {
            UserId = userId,
            ProductId = request.ProductId,
            Quantity = request.Quantity,
            TotalPrice = totalPrice,
            OrderDate = DateTime.UtcNow
        };

        _db.Orders.Add(order);
        await _db.SaveChangesAsync();

        return new OrderResponseDto(
            order.Id,
            product.Id,
            product.Name,
            order.Quantity,
            order.TotalPrice,
            order.OrderDate
        );
    }
}
