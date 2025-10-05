namespace Store.Data.Entities.OrderEntities;

public class Order: BaseEntity<Guid>
{
    public string BuyerEmail { get; set; }
    public DateTimeOffset OrderDate { get; set; } = DateTimeOffset.Now;
    public ShippingAddress ShipToAddress { get; set; } 
    public DeliveryMethod DeliveryMethod { get; set; }
    public int? DeliveryMethodId { get; set; }
    public OrderStatus OrderStatus { get; set; } = OrderStatus.Placed;
    public OrderPaymentStatus OrderPaymentStatus { get; set; } = OrderPaymentStatus.Pending;
    public IReadOnlyList<OrderItem> OrderItems { get; set; }
    public string? BasketId { get; set; }
    public decimal Subtotal { get; set; }
    public decimal GetTotal => Subtotal + (DeliveryMethod?.Price ?? 0);
    public string? PaymentIntentId { get; set; }
}