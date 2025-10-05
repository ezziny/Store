
using System.ComponentModel.DataAnnotations;

namespace Store.Services.Services.UserService.Dtos;

public class RegisterDto
{
    [Required]
    public string DisplayName { get; set; }
    [EmailAddress]
    [Required]
    public string Email { get; set; }
[RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[\W_]).{8,}$", 
    ErrorMessage = "Password must be at least 8 characters long and include uppercase, lowercase, digit, and special character.")]
    public string Password { get; set; }
}