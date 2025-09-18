using System.Text.Json;
using StackExchange.Redis;

namespace Store.Services.Services.CacheService;

public class CacheService : ICacheService
{
    private readonly IDatabase _database;
    public CacheService(IConnectionMultiplexer redis)
    {
        _database = redis.GetDatabase();
    }
    public async Task<string> GetCachedResponseAsync(string key)
    {
        var cachedResponse = await _database.StringGetAsync(key);
        if (cachedResponse.IsNullOrEmpty) return null!;
        return cachedResponse.ToString();
    }

    public async Task SetCachedResponseAsync(string key, object response, TimeSpan timeToLive)
    {
        if (response is null)
            return;
        var json = JsonSerializer.Serialize(response, new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase });
        await _database.StringSetAsync(key, json, timeToLive);
    }
}
