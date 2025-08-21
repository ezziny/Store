using System;
using AutoMapper;
using Microsoft.Extensions.Configuration;

namespace Store.Services.Services.Product.Dtos;

public class ProductPictureUrlResolver : IValueResolver<Data.Entities.Product, ProductDetailsDto, string>
{
    private readonly IConfiguration _configuration;

    public ProductPictureUrlResolver(IConfiguration configuration)
    {
        _configuration = configuration;
    }
    public string Resolve(Data.Entities.Product source, ProductDetailsDto destination, string destMember, ResolutionContext context)
    {
        if (!string.IsNullOrEmpty(source.PictureUrl)) return $"{_configuration["BaseUrl"]}/{source.PictureUrl}";
        return null;
    }
}
