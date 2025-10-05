using System.ComponentModel.DataAnnotations;

namespace Store.Services.Services.UserService.Dtos;

public class LoginDto
{
    [EmailAddress]
    [Required]
    public string Email { get; set; }
    [Required]
    public string Password { get; set; }
}