using System;
using AutoMapper;
using Store.Data.Entities.Basket;

namespace Store.Services.Services.BasketService.Dtos;

public class BasketProfile : Profile
{
    public BasketProfile()
    {
        CreateMap<BasketItem, BasketItemDto>().ReverseMap();
        CreateMap<CustomerBasket, CustomerBasketDto>().ReverseMap();
    }
}
