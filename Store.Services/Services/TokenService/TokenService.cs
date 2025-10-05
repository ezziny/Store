using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Store.Data.Entities.IdentityEntities;

namespace Store.Services.Services.TokenService;

public class TokenService : ITokenService
{
    private readonly IConfiguration _configuration;
    private readonly SymmetricSecurityKey _key;

    public TokenService(IConfiguration configuration)
    {
        _configuration = configuration;
        _key = new(Encoding.UTF8.GetBytes(configuration["Token:Key"]));
    }
    public string GenerateToken(AppUser user)
    {
        JwtSecurityTokenHandler tokenHandler = new();
        SigningCredentials credentials = new(_key, SecurityAlgorithms.HmacSha512);
        List<Claim> claims = [
            new(ClaimTypes.Email, user.Email),
            new(ClaimTypes.GivenName, user.DisplayName),
            new("UserId", user.Id),
            new("UserName", user.UserName),
        ];
        SecurityTokenDescriptor tokenDescriptor = new()
        {
            Subject = new ClaimsIdentity(claims),
            Issuer = _configuration["Token:Issuer"],
            IssuedAt = DateTime.Now,
            Expires = DateTime.Now.AddDays(2),
            NotBefore = new DateTime(2025, 1, 1),
            SigningCredentials = credentials
        };
        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }
}
