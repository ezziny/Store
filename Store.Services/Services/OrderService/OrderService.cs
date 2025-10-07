using AutoMapper;
using Store.Data.Entities;
using Store.Data.Entities.Basket;
using Store.Data.Entities.OrderEntities;
using Store.Repository.Interfaces;
using Store.Repository.Specifications.OrderSpecification;
using Store.Services.Services.BasketService;
using Store.Services.Services.OrderService.Dtos;
using Store.Services.Services.PaymentService;

namespace Store.Services.Services.OrderService;

public class OrderService : IOrderService
{
    private readonly IBasketService _basketService;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly IPaymentService _paymentService;

    public OrderService(IBasketService basketService, IUnitOfWork unitOfWork, IMapper mapper, IPaymentService paymentService)
    {
        _basketService = basketService;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _paymentService = paymentService;
    }
    
    public async Task<OrderDetailsDto> CreateOrderAsync(OrderDto input)
    {
        var basket = await _basketService.GetBasketAsync(input.BasketId);
        if (basket is null) throw new Exception("Basket not found");
        // transfer items from basket to order

        #region AddingItemsToOrder

        var orderItems = new List<OrderItem>();
        foreach (var basketItem in basket.BasketItems)
        {
            var productItem = await _unitOfWork.Repository<Data.Entities.Product, int>().GetByIdAsync(basketItem.ProductId);
            if (productItem is null) throw new Exception($"Product with id {basketItem.ProductId} not found");
            var itemOrdered = new ProductItem
            {
                ProductId = productItem.Id,
                ProductName = productItem.Name,
                PictureUrl = productItem.PictureUrl
            };
            var orderItem = new OrderItem
            {
                Price = productItem
                    .Price, //getting it from the product table to avoid manipulation from the client side
                Quantity = basketItem.Quantity,
                ProductItem = itemOrdered
            };
            orderItems.Add(orderItem);

        }
        #endregion

        #region GettingDeliveryMethod

        var deliveryMethod = await _unitOfWork.Repository<DeliveryMethod, int>().GetByIdAsync(input.DeliveryMethod);
        if  (deliveryMethod is null) throw new Exception($"Delivery method with id {input.DeliveryMethod} not found");
        #endregion

        #region CalculateSubTotal
        var subTotal = orderItems.Sum(orderItem => orderItem.Price * orderItem.Quantity);
        #endregion
        // TODO PAYMENT

        #region Payment
        var specs = new OrderWithPaymentIntentSpecification(basket.PaymentIntentId);
        var existingOrder = await _unitOfWork.Repository<Order, Guid>().GetWithSpecificationByIdAsync(specs);
        if (existingOrder is null)
            await _paymentService.CreateOrUpdatePaymentIntent(basket);
        
        #endregion

        #region CreateOrder
        var mappedShippingAdress = _mapper.Map<ShippingAddress>(input.ShippingAdress);
        var mappedOrderItems = _mapper.Map<List<OrderItem>>(orderItems);
        Order order = new()
        {
            BuyerEmail = input.BuyerEmail,
            ShipToAddress = mappedShippingAdress,
            OrderItems = mappedOrderItems,
            BasketId = input.BasketId,
            DeliveryMethodId = deliveryMethod.Id,
            Subtotal = subTotal,
            PaymentIntentId = basket.PaymentIntentId
        };
        await _unitOfWork.Repository<Order, Guid>().AddAsync(order);
        await _unitOfWork.CompleteAsync();
        return _mapper.Map<OrderDetailsDto>(order);
        #endregion
    }

    public async Task<IReadOnlyList<OrderDetailsDto>> GetOrdersForUserAsync(string buyerEmail)
    {
        var specs = new OrderWithItemSpecification(buyerEmail);
        var orders = await _unitOfWork.Repository<Order, Guid>().GetAllWithSpecificationAsync(specs);
        return (orders.Count > 0) ? _mapper.Map<List<OrderDetailsDto>>(orders): throw new Exception("no orders found");
    }

    public async Task<OrderDetailsDto> GetOrderByIdAsync(Guid id) {
        var specs = new OrderWithItemSpecification(id);
        var orders = await _unitOfWork.Repository<Order, Guid>().GetWithSpecificationByIdAsync(specs);
        return (orders is not null) ? _mapper.Map<OrderDetailsDto>(orders): throw new Exception($"order with id {id} not found");
    }
    public async Task<IReadOnlyList<DeliveryMethod>> GetDeliveryMethodsAsync() =>
        await _unitOfWork.Repository<DeliveryMethod, int>().GetAllAsync();
}