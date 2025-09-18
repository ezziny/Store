using System.ComponentModel.DataAnnotations;

namespace Store.Services.Services.BasketService.Dtos;

public class BasketItemDto
{
    [Required]
    [Range(1,int.MaxValue)]
    public int ProductId { get; set; }
    [Required]
    public string ProductName { get; set; }
    [Required]
    [Range(0.1,double.MaxValue, ErrorMessage ="price must be equal to or greater than 0.1")]
    public decimal Price { get; set; }
    [Required]
    [Range(1, 12, ErrorMessage = "you may only order from 1 to 12 pieces at a time")]
    public int Quantity { get; set; }
    [Required]
    public string PictureUrl { get; set; }
    [Required]
    public string BrandName { get; set; }
    [Required]
    public string TypeName { get; set; }

}
