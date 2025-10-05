using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Store.Data.Entities.IdentityEntities;
using Store.Services.HandleResponses;
using Store.Services.Services.UserService;
using Store.Services.Services.UserService.Dtos;

namespace Store.Web.Controllers;
[AllowAnonymous]

public class AccountController:BaseController
{
    private readonly IUserService _userService;
    private readonly UserManager<AppUser> _userManager;

    public AccountController(IUserService userService, UserManager<AppUser> userManager)
    {
        _userService = userService;
        _userManager = userManager;
    }

    [HttpPost]
    public async Task<ActionResult<UserDto>> Login([FromBody] LoginDto input)
    {
        var result = await _userService.Login(input);
        if (result == null) return BadRequest(new CustomException(400, "Email Doesn't exist"));
        return Ok(result);
    }

    [HttpPost]
    public async Task<ActionResult<UserDto>> Register([FromBody] RegisterDto input)
    {
        var result = await _userService.Register(input);
        if (result == null) return BadRequest(new CustomException(400, "Email Already exists"));
        return Ok(result);
    }

    [HttpGet]
    public async Task<UserDto> GetCurrentUserDetails()
    {
        var UserId = User.FindFirst("UserId")?.Value;
        var user = await _userManager.FindByIdAsync(UserId!);
        return new UserDto
        {
            Id = Guid.Parse(user!.Id),
            Email = user.Email!,
            DisplayName = user.DisplayName,

        };
    }
}