using Marketplace.Application.DTOs;

namespace Marketplace.Application.Interfaces;

public interface IOrderService
{
    Task<OrderResponseDto> CreateOrderAsync(int userId, CreateOrderDto request);
}
