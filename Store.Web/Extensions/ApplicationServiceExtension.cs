using Microsoft.AspNetCore.Mvc;
using Store.Repository.Interfaces;
using Store.Repository.Repositories;
using Store.Services.HandleResponses;
using Store.Services.Services.BasketService;
using Store.Services.Services.BasketService.Dtos;
using Store.Services.Services.CacheService;
using Store.Services.Services.OrderService;
using Store.Services.Services.OrderService.Dtos;
using Store.Services.Services.PaymentService;
using Store.Services.Services.Product;
using Store.Services.Services.Product.Dtos;
using Store.Services.Services.TokenService;
using Store.Services.Services.UserService;

namespace Store.Web.Extensions;

public static class ApplicationServiceExtension
{
    public static IServiceCollection AddAplicationServices(this IServiceCollection services)
    {
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen();
        services.AddScoped<IUnitOfWork,UnitOfWork>();
        services.AddScoped<IBasketRepository,BasketRepository>();
        services.AddScoped<ITokenService,TokenService>();
        services.AddScoped<IUserService,UserService>();
        services.AddAutoMapper(typeof(ProductProfile));
        services.AddAutoMapper(typeof(BasketProfile));
        services.AddAutoMapper(typeof(OrderProfile));
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
        services.AddScoped<IBasketService, BasketService>();
        services.AddScoped<IOrderService, OrderService>();
        services.AddScoped<IPaymentService, PaymentService>();

        return services;
    }
}
