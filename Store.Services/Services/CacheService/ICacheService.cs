namespace Store.Services.Services.CacheService;

public interface ICacheService
{
    Task SetCachedResponseAsync(string key, object response, TimeSpan timeToLive);
    Task<string> GetCachedResponseAsync(string key);
}