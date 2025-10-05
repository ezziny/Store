using Store.Data.Entities;
using Store.Services.Services.OrderService.Dtos;

namespace Store.Services.Services.OrderService;

public interface IOrderService
{
    Task<OrderDetailsDto> CreateOrderAsync(OrderDto input);
    Task<IReadOnlyList<OrderDetailsDto>> GetOrdersForUserAsync(string buyerEmail);
    Task<OrderDetailsDto> GetOrderByIdAsync(Guid id);
    Task<IReadOnlyList<DeliveryMethod>> GetDeliveryMethodsAsync();
}