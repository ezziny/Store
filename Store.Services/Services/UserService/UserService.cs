using Microsoft.AspNetCore.Identity;
using Store.Data.Entities.IdentityEntities;
using Store.Services.Services.TokenService;
using Store.Services.Services.UserService.Dtos;

namespace Store.Services.Services.UserService;

public class UserService:IUserService
{
    private readonly SignInManager<AppUser> _signInManager;
    private readonly UserManager<AppUser> _userManager;
    private readonly ITokenService _tokenService;

    public UserService(SignInManager<AppUser> signInManager, UserManager<AppUser> userManager, ITokenService tokenService)
    {
        _signInManager = signInManager;
        _userManager = userManager;
        _tokenService = tokenService;
    }
    public async Task<UserDto> Login(LoginDto input)
    {
        var user = await _userManager.FindByEmailAsync(input.Email);
        if (user == null) return null!;
        var result = await _signInManager.CheckPasswordSignInAsync(user, input.Password, false);
        if (result.Succeeded)
        {
            return new UserDto
            {
                Id = Guid.Parse(user.Id),
                Email = user.Email!,
                DisplayName = user.DisplayName,
                Token = _tokenService.GenerateToken(user)
            };
        }
        throw new Exception("Invalid login attempt");
    }

    public async Task<UserDto> Register(RegisterDto input)
    {
        var user = await _userManager.FindByEmailAsync(input.Email);
        if (user != null) return null;
        user = new AppUser
        {
            DisplayName = input.DisplayName,
            Email = input.Email,
            UserName = input.Email.Split("@")[0],
        };
        var result = await _userManager.CreateAsync(user, input.Password);
        if (!result.Succeeded)
        {
            throw new Exception("Registration failed");
        }
        return new UserDto
        {
            Id = Guid.Parse(user.Id),
            Email = user.Email!,
            DisplayName = user.DisplayName,
            Token = _tokenService.GenerateToken(user)
        };
    }
}