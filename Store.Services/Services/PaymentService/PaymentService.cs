using AutoMapper;
using Microsoft.Extensions.Configuration;
using Store.Data.Entities;
using Store.Data.Entities.OrderEntities;
using Store.Repository.Interfaces;
using Store.Repository.Specifications.OrderSpecification;
using Store.Services.Services.BasketService;
using Store.Services.Services.BasketService.Dtos;
using Store.Services.Services.OrderService.Dtos;
using Stripe;
using Stripe.Terminal;

namespace Store.Services.Services.PaymentService;

public class PaymentService: IPaymentService
{
    private readonly IConfiguration _configuration;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IBasketService _basketService;
    private readonly IMapper _mapper;

    public PaymentService(
        IConfiguration  configuration,
        IUnitOfWork unitOfWork,
        IBasketService basketService,
        IMapper mapper
        )
    {
        _configuration = configuration;
        _unitOfWork = unitOfWork;
        _basketService = basketService;
        _mapper = mapper;
    }
    public async Task<CustomerBasketDto> CreateOrUpdatePaymentIntent(CustomerBasketDto basket)
    {
        StripeConfiguration.ApiKey = _configuration["Stripe:SecretKey"];
        if (basket is null) throw new Exception("Empty Basket");
        var deliveryMethod = await _unitOfWork.Repository<DeliveryMethod, int>().GetByIdAsync(basket.DeliveryMethodId??throw new Exception("Delivery method not provided"));
        decimal shippingPrice = deliveryMethod.Price;
        foreach (var item in basket.BasketItems)
        {
            var product = await _unitOfWork.Repository<Data.Entities.Product, int>().GetByIdAsync(item.ProductId);
            if (item.Price != product.Price)
                item.Price = product.Price;
        }

        var service = new PaymentIntentService();
        PaymentIntent paymentIntent;
        if (string.IsNullOrEmpty(basket.PaymentIntentId))
        {
            var options = new PaymentIntentCreateOptions
            {
                Amount = (long)basket.BasketItems.Sum(i => ((i.Price) * (i.Quantity) + (long)shippingPrice)*100),
                Currency = "usd",
                PaymentMethodTypes = new List<string>{"card"}
            };
            paymentIntent = await service.CreateAsync(options);
            basket.PaymentIntentId = paymentIntent.Id;
            basket.ClientSecret = paymentIntent.ClientSecret;
        }
        else
        {
            var options = new PaymentIntentUpdateOptions
            {
                Amount = (long)basket.BasketItems.Sum(i => ((i.Price) * (i.Quantity) + (long)shippingPrice) * 100),
            };
            await service.UpdateAsync(basket.PaymentIntentId, options);
        }

        await _basketService.UpdateBasketAsync(basket);
        return basket;
    }

    public async Task<OrderDetailsDto> UpdateOrderPaymentSucceeded(string paymentIntentId)
    {
        return await UpdateOrderPaymentStatus(paymentIntentId, OrderPaymentStatus.Received, async order =>
        {
            await _basketService.DeleteBasketAsync(order.BasketId);
        });
    }

    public async Task<OrderDetailsDto> UpdateOrderPaymentFailed(string paymentIntentId)
    {
        return await UpdateOrderPaymentStatus(paymentIntentId, OrderPaymentStatus.Failed);
    }

    //im really proud of this one hehehe
    private async Task<OrderDetailsDto> UpdateOrderPaymentStatus(
        string paymentIntentId,
        OrderPaymentStatus status,
        Func<Order, Task>? extraAction = null)
    {
        var specs = new OrderWithPaymentIntentSpecification(paymentIntentId);
        var order = await _unitOfWork.Repository<Order, Guid>().GetWithSpecificationByIdAsync(specs);
        if (order is null) throw new Exception("Order not found");
        order.OrderPaymentStatus = status;
        _unitOfWork.Repository<Order, Guid>().Update(order);
        if (extraAction != null)
            await extraAction(order);
        await _unitOfWork.CompleteAsync();
        return _mapper.Map<OrderDetailsDto>(order);
    }
}