using System;
using AutoMapper;
using Store.Data.Entities.Basket;
using Store.Repository.Interfaces;
using Store.Services.Services.BasketService.Dtos;

namespace Store.Services.Services.BasketService;

public class BasketService : IBasketService
{
    private readonly IBasketRepository _basketRepository;
    private readonly IMapper _mapper;

    public BasketService(IBasketRepository basketRepository, IMapper mapper)
    {
        _basketRepository = basketRepository;
        _mapper = mapper;
    }
    public async Task<bool> DeleteBasketAsync(string BasketId) => await _basketRepository.DeleteBasketAsync(BasketId);
    public async Task<CustomerBasketDto> GetBasketAsync(string basketId)
    {
        var basket = await _basketRepository.GetBasketAsync(basketId);
        if (basket is null)
            return new CustomerBasketDto();
        else
        {
            return _mapper.Map<CustomerBasketDto>(basket);
        }
    }
    public async Task<CustomerBasketDto> UpdateBasketAsync(CustomerBasketDto input)
    {
        if (input.Id is null)
        {
            input.Id = GenerateRandomBasketId();
        }
        var customerBasket = _mapper.Map<CustomerBasket>(input);
        var updatedBasket = await _basketRepository.UpdateBasketAsync(customerBasket);
        var mappedUpdatedBasket = _mapper.Map<CustomerBasketDto>(updatedBasket);
        return mappedUpdatedBasket;
    }
    private string GenerateRandomBasketId()
    {
        Random random = new();
        int randomDigits = random.Next(1000, 9999);
        return $"bskt-{randomDigits}";
    }
}
