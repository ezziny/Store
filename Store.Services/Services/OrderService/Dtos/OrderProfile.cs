using AutoMapper;
using Store.Data.Entities.OrderEntities;

namespace Store.Services.Services.OrderService.Dtos;

public class OrderProfile : Profile
{
    public OrderProfile ()
    {
        CreateMap<ShippingAddress, AddressDto>().ReverseMap();
        CreateMap<Order,OrderDetailsDto>()
            .ForMember(d => d.DeliveryMethodName, o => o.MapFrom(s => s.DeliveryMethod.ShortName))
            .ForMember(d => d.ShippingPrice, o => o.MapFrom(s => s.DeliveryMethod.Price));
        CreateMap<OrderItem, OrderItemDto>()
            .ForMember(d => d.ProductItemId, o => o.MapFrom(s => s.ProductItem.ProductId))
            .ForMember(d => d.ProductName, o => o.MapFrom(s => s.ProductItem.ProductName))
            .ForMember(d => d.PictureUrl, o => o.MapFrom(s => s.ProductItem.PictureUrl))
            .ForMember(d => d.PictureUrl, o => o.MapFrom<OrderItemPictureUrlResolver>());


    }
}