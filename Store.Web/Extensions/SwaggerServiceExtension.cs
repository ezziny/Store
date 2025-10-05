using Microsoft.OpenApi.Models;

namespace Store.Web.Extensions;

public static class SwaggerServiceExtension
{
    public static IServiceCollection AddSwaggerDocumentation(this IServiceCollection services)
    {
        services.AddSwaggerGen(options =>
        {
            options.SwaggerDoc("v1", new OpenApiInfo
            {
                Title = "Store API Documentation (hopefully)",
                Version = "V1",
                TermsOfService = new Uri("https://chatgpt.com/")
            });
            var securitySchema = new OpenApiSecurityScheme
            {
                Description = "JWT Authentication using Bearer scheme. Example: \"Bearer {token}\"",
                Name = "Authorization",
                In = ParameterLocation.Header,
                Type = SecuritySchemeType.ApiKey,
                Scheme = "Bearer",
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            };
            options.AddSecurityDefinition("Bearer", securitySchema);
            var securityRequirement = new OpenApiSecurityRequirement
            {
                {securitySchema, ["Bearer"] }
            };
            options.AddSecurityRequirement(securityRequirement);
        });
        return services;
    }
}