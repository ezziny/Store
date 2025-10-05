using Store.Data.Entities.IdentityEntities;

namespace Store.Services.Services.TokenService;

public interface ITokenService
{
    string GenerateToken(AppUser user);
}
