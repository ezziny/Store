using System.ComponentModel.DataAnnotations;

namespace Store.Services.Services.OrderService.Dtos;

public class OrderDto
{
    public string BasketId { get; set; }
    public string BuyerEmail { get; set; }
    [Required]
    public int DeliveryMethod { get; set; }

    public AddressDto ShippingAdress { get; set; }
}