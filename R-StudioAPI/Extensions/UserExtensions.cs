using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using R_StudioAPI.Models;

namespace R_StudioAPI.Extensions
{
    public static class UserExtensions
    {
        public async static Task<bool> IsAdmin(this User user, UserManager<User> userManager)
        {
            return (await userManager.GetRolesAsync(user)).Contains("Admin");
        }
    }
}
