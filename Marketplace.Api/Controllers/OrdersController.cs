using System.Security.Claims;
using Marketplace.Application.DTOs;
using Marketplace.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Marketplace.Api.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class OrdersController : ApiControllerBase
{
    private readonly IOrderService _orderService;

    public OrdersController(IOrderService orderService)
    {
        _orderService = orderService;
    }

    [HttpPost] // POST: api/orders
    public async Task<IActionResult> CreateOrder([FromBody] CreateOrderDto request)
    {
        var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);

        if (!int.TryParse(userIdString, out int userId))
            return Unauthorized(new { Message = "Invalid user token." });

        var result = await _orderService.CreateOrderAsync(userId, request);

        return result.IsSuccess ? Ok(result.Value) : HandleFailure(result);
    }

    [HttpGet] // GET: api/orders
    public async Task<ActionResult<IEnumerable<OrderResponseDto>>> GetOrders()
    {
        var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);

        if (!int.TryParse(userIdString, out int userId))
            return Unauthorized(new { Message = "Invalid user token." });

        var result = await _orderService.GetOrdersAsync(userId);

        return result.IsSuccess ? Ok(result.Value) : HandleFailure(result);
    }
}
