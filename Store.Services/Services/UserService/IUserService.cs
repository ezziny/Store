using Store.Services.Services.UserService.Dtos;

namespace Store.Services.Services.UserService;

public interface IUserService
{
    Task<UserDto> Login(LoginDto input);
    Task<UserDto> Register(RegisterDto input);
}