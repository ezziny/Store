using AutoMapper;
using Store.Data.Entities;

namespace Store.Services.Services.Product.Dtos;

public class ProductProfile : Profile
{
    public ProductProfile()
    {
        CreateMap<Data.Entities.Product, ProductDetailsDto>()
            .ForMember(dest => dest.BrandName, options => options.MapFrom(src => src.Brand.Name))
            .ForMember(dest => dest.TypeName, options => options.MapFrom(src => src.Type.Name))
            .ForMember(dest => dest.PictureUrl, options => options.MapFrom<ProductPictureUrlResolver>());
        CreateMap<ProductBrand, BrandTypeDetailsDto>();
        CreateMap<ProductType, BrandTypeDetailsDto>();
    }
}
