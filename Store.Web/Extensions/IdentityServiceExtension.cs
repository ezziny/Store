using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using Store.Data.Contexts;
using Store.Data.Entities.IdentityEntities;


namespace Store.Web.Extensions;

public static class IdentityServiceExtension
{
    public static IServiceCollection AddIdentityServices(this IServiceCollection services, IConfiguration _configuration)
    {
        var builder = services.AddIdentityCore<AppUser>();
        builder = new IdentityBuilder(builder.UserType, builder.Services);
        builder.AddEntityFrameworkStores<StoreIdentityDbContext>();
        builder.AddSignInManager<SignInManager<AppUser>>();
        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(_configuration["Token:Key"]!)),
                    ValidIssuer = _configuration["Token:Issuer"],
                    ValidateIssuer = true,
                    ValidateAudience = false
                };
            })
            ;
        return services;
    }
}
