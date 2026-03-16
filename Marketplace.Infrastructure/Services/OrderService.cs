using Marketplace.Application.Common;
using Marketplace.Application.DTOs;
using Marketplace.Application.Interfaces;
using Marketplace.Domain.Entities;
using Marketplace.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Marketplace.Infrastructure.Services;

public class OrderService : IOrderService
{
    private readonly MarketplaceDbContext _db;

    public OrderService(MarketplaceDbContext db)
    {
        _db = db;
    }

    public async Task<Result<OrderResponseDto>> CreateOrderAsync(int userId, CreateOrderDto request)
    {
        var product = await _db.Products.FindAsync(request.ProductId);
        if (product == null)
            return Result<OrderResponseDto>.Failure($"Product with ID {request.ProductId} was not found.", ErrorType.NotFound);

        if (product.StockQuantity < request.Quantity)
            return Result<OrderResponseDto>.Failure($"Not enough stock. Only {product.StockQuantity} available.", ErrorType.BadRequest);

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

        try
        {
            await _db.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            return Result<OrderResponseDto>.Failure(
                "The product stock was just updated by another user. Please review the available stock and try again.",
                ErrorType.Conflict
            );
        }

        var responseDto = new OrderResponseDto(
            order.Id,
            product.Id,
            product.Name,
            order.Quantity,
            order.TotalPrice,
            order.OrderDate
        );
        return Result<OrderResponseDto>.Success(responseDto);
    }

    public async Task<Result<IEnumerable<OrderResponseDto>>> GetOrdersAsync(int userId)
    {
        var orders = await _db.Orders
            .AsNoTracking()
            .Include(o => o.Product)
            .Where(o => o.UserId == userId)
            .OrderByDescending(o => o.OrderDate)
            .ToListAsync();

        var responseDtos = orders.Select(o => new OrderResponseDto(
            o.Id,
            o.ProductId,
            o.Product.Name,
            o.Quantity,
            o.TotalPrice,
            o.OrderDate
        ));
        return Result<IEnumerable<OrderResponseDto>>.Success(responseDtos);
    }
}
