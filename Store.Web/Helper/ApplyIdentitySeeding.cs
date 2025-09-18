using System;
using Microsoft.AspNetCore.Identity;
using Store.Data.Entities.IdentityEntities;

namespace Store.Web.Helper;

public class ApplyIdentitySeeding
{
    public static async Task SeedUserAsync(UserManager<AppUser> userManager)
    {
        if (!userManager.Users.Any())
        {
            var user = new AppUser
            {
                DisplayName = "ezziny",
                Email = "ezziny@gmail.com",
                UserName = "ezziny",
                Address = new Address
                {
                    FirstName = "Ezziny",
                    LastName = "Mohamed",
                    Street = "123 Main St",
                    City = "Cairo",
                    State = "Cairo",
                    PostalCode = "12345"
                }
            };
            await userManager.CreateAsync(user, "Ezziny1@");
        }
    }
}