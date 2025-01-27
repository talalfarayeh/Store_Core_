using Microsoft.AspNetCore.Identity;
using Store.Infrastructure.Models;
using System.Threading.Tasks;

namespace Store.Core.Application.Sarvice
{
    public class AuthService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public AuthService(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
        }

        public async Task<bool> RegisterUser(string email, string password, string fullName)
        {
            var user = new ApplicationUser { UserName = email, Email = email, FullName = fullName };
            var result = await _userManager.CreateAsync(user, password);

            if (!result.Succeeded)
            {
                return false;
            }

             if (!await _roleManager.RoleExistsAsync("User"))
            {
                await _roleManager.CreateAsync(new IdentityRole("User"));
            }

            await _userManager.AddToRoleAsync(user, "User");

             await _signInManager.SignInAsync(user, isPersistent: false);
            return true;
        }

        public async Task<bool> LoginUser(string email, string password)
        {
             var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
            {
                return false;
            }

             var result = await _signInManager.PasswordSignInAsync(user.Email, password, false, false);
            return result.Succeeded;
        }

        public async Task Logout()
        {
            await _signInManager.SignOutAsync();
        }
    }
}
