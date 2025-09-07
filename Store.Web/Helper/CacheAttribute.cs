using System;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Store.Services.Services.CacheService;

namespace Store.Web.Helper;

public class CacheAttribute : Attribute, IAsyncActionFilter
{
    private readonly int _timeToLive;

    public CacheAttribute(int timeToLive)
    {
        _timeToLive = timeToLive;
    }

    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        var _cacheService = context.HttpContext.RequestServices.GetRequiredService<ICacheService>();
        var cacheKey = GenerateCacheKeyFromRequest(context.HttpContext.Request);
        var cachedResponse = await _cacheService.GetCachedResponseAsync(cacheKey);
        if (!string.IsNullOrEmpty(cachedResponse))
        {
            var contentResult = new ContentResult
            {
                Content = cachedResponse,
                ContentType = "application/json",
                StatusCode = 200
            };
            context.Result = contentResult;
            return;
        }
        var executedContext = await next();
        if (executedContext.Result is OkObjectResult okObject)
        {
            await _cacheService.SetCachedResponseAsync(cacheKey, okObject.Value!, TimeSpan.FromMinutes(_timeToLive));
        }
    }
    public string GenerateCacheKeyFromRequest(HttpRequest request)
    {
        StringBuilder cacheKey = new();
        cacheKey.Append($"{request.Path}");
        foreach (var (key, value) in request.Query.OrderBy(x => x.Key))
        {
            cacheKey.Append($"|{key}-يابوالعباااااااااااس{value}");
        }
        return cacheKey.ToString();
    }
}
