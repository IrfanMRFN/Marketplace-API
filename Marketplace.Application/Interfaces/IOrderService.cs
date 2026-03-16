using Marketplace.Application.Common;
using Marketplace.Application.DTOs;

namespace Marketplace.Application.Interfaces;

public interface IOrderService
{
    Task<Result<OrderResponseDto>> CreateOrderAsync(int userId, CreateOrderDto request);
    Task<Result<IEnumerable<OrderResponseDto>>> GetOrdersAsync(int userId);
}
