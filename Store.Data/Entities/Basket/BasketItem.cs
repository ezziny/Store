namespace Store.Data.Entities.Basket;

public class BasketItem
{
    public int ProductId { get; set; }
    public string ProductName { get; set; }
    public decimal Price { get; set; }
    public int Quantity { get; set; }
    public string PictureUrl { get; set; }
    public string BrandName { get; set; }
    public string TypeName { get; set; }
}