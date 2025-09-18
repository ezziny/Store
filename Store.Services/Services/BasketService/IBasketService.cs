using System;
using Store.Services.Services.BasketService.Dtos;

namespace Store.Services.Services.BasketService;

public interface IBasketService
{
    Task<CustomerBasketDto> GetBasketAsync(string basketId);
    Task<CustomerBasketDto> UpdateBasketAsync(CustomerBasketDto basket);
    Task<bool> DeleteBasketAsync(string BasketId);

}
