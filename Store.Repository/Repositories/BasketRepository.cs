using System.Text.Json;
using StackExchange.Redis;
using Store.Data.Entities.Basket;
using Store.Repository.Interfaces;

namespace Store.Repository.Repositories;

public class BasketRepository : IBasketRepository
{
    private readonly IDatabase _database;

    public BasketRepository(IConnectionMultiplexer redis)
    {
        _database= redis.GetDatabase();
    }
    public async Task<bool> DeleteBasketAsync(string BasketId)
    {
        return await _database.KeyDeleteAsync(BasketId);
    }

    public async Task<CustomerBasket> GetBasketAsync(string basketId)
    //cool one-liner but performance is ass because you're making the call twice
    // => string.IsNullOrEmpty(await _database.StringGetAsync(basketId)) ? JsonSerializer.Deserialize<CustomerBasket>(await _database.StringGetAsync(basketId)) : null;
    {
        var data = await _database.StringGetAsync(basketId);
        return !string.IsNullOrEmpty(data)?JsonSerializer.Deserialize<CustomerBasket>(data):null;
    }

    public async Task<CustomerBasket> UpdateBasketAsync(CustomerBasket basket)
    {
        var data = await _database.StringSetAsync(basket.Id, JsonSerializer.Serialize(basket), TimeSpan.FromMinutes(120));
        if (!data) return null;
        return await GetBasketAsync(basket.Id);
    }
}
