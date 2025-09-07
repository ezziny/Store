using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Store.Data.Contexts;
using Store.Repository.Interfaces;
using Store.Repository.Repositories;
using Store.Services.HandleResponses;
using Store.Services.Services.CacheService;
using Store.Services.Services.Product;
using Store.Services.Services.Product.Dtos;

namespace Store.Web.Extensions;

public static class ApplicationServiceExtension
{
    public static IServiceCollection AddAplicationServices(this IServiceCollection services)
    {
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen();
        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddAutoMapper(typeof(ProductProfile));
        services.Configure<ApiBehaviorOptions>(options =>
        {
            options.InvalidModelStateResponseFactory = actionContext =>
            {
                var errors = actionContext.ModelState.Where(model => model.Value?.Errors.Count > 0)
                            .SelectMany(model => model.Value!.Errors)
                            .Select(error => error.ErrorMessage)
                            .ToList();
                var errorResonse = new ValidationErrorResponse { Errors = errors };
                return new BadRequestObjectResult(errorResonse);
            };
        });
        services.AddScoped<IProductService, ProductService>();
        services.AddScoped<ICacheService, CacheService>();

        return services;
    }
}
