using System;
using System.Net;
using System.Text.Json;
using Store.Services.HandleResponses;

namespace Store.Web.Middlewares;

public class ExceptionMiddleware
{
    private readonly RequestDelegate _next;
    private readonly IHostEnvironment _environment;
    private readonly ILogger<ExceptionMiddleware> _logger;

    public ExceptionMiddleware(RequestDelegate next, IHostEnvironment environment, ILogger<ExceptionMiddleware> logger)
    {
        _next = next;
        _environment = environment;
        _logger = logger;
    }
    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception e)
        {
            _logger.LogError(e, e.Message);
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            var response = _environment.IsDevelopment() ? new CustomException((int)HttpStatusCode.InternalServerError, e.Message, e.StackTrace) : new CustomException((int)HttpStatusCode.InternalServerError);
            var json = JsonSerializer.Serialize(response, new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase });
            await context.Response.WriteAsync(json);
        }
    }
}
