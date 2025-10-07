using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Store.Services.HandleResponses;
using Store.Services.Services.OrderService;
using Store.Services.Services.OrderService.Dtos;

namespace Store.Web.Controllers;

public class OrderController:BaseController
{
    private readonly IOrderService _orderService;

    public OrderController(IOrderService OrderService)
    {
        _orderService = OrderService;
    }

    [HttpPost]
    public async Task<ActionResult<OrderDetailsDto>> CreateOrderAsync(OrderDto input)
    {
        var order = await _orderService.CreateOrderAsync(input);
        if (order is null) return BadRequest(new Response(400, "Error while creating your order"));
        return Ok(order);
    }

    [HttpGet]
    public async Task<ActionResult<OrderDetailsDto>> GetAllOrdersForUserAsync()
    {
        var email = User.FindFirstValue(ClaimTypes.Email);
        if (email is null) throw new Exception("how in the world do you have no email???");
        var orders = await _orderService.GetOrdersForUserAsync(email);
        return Ok(orders);
    }
    [HttpGet]
    public async Task<ActionResult<OrderDetailsDto>> GetOrderByIdAsync(Guid id)
    {
        var orders = await _orderService.GetOrderByIdAsync(id);
        return Ok(orders);
    }

    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<OrderDetailsDto>>> GetAllDeliveryMethodsAsync() =>
        Ok(await _orderService.GetDeliveryMethodsAsync());
}