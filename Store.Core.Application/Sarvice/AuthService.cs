using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Store.Infrastructure.Models;
using System.Security.Claims;
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

        public async Task<IdentityResult> RegisterUser(string email, string password, string fullName)
        {
            var user = new ApplicationUser
            {
                UserName = email,
                Email = email,
                FullName = fullName
            };

            var result = await _userManager.CreateAsync(user, password);
            if (!result.Succeeded)
            {
                return result; // إرجاع الأخطاء إذا فشل التسجيل
            }

            // تعيين دور المستخدم
            await _userManager.AddToRoleAsync(user, "User");

            // تسجيل الدخول بعد التسجيل مباشرة
            await _signInManager.SignInAsync(user, isPersistent: false);

            return result;
        }




        public async Task<bool> LoginUser(string email, string password)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
            {
                return false;
            }

            var result = await _signInManager.PasswordSignInAsync(user.UserName, password, false, false);
            if (!result.Succeeded)
            {
                return false;
            }

            // ✅ تحميل الصورة الافتراضية إذا لم يكن لدى المستخدم صورة
            var profilePicturePath = string.IsNullOrEmpty(user.ProfilePicture) ? "/uploads/default-avatar.png" : $"/{user.ProfilePicture}";

            var claims = new List<Claim>
    {
        new Claim(ClaimTypes.Name, user.UserName),
        new Claim("FullName", user.FullName ?? "User"),
        new Claim("ProfilePicture", profilePicturePath)
    };

            var claimsIdentity = new ClaimsIdentity(claims, "ApplicationCookie");
            var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);

            await _signInManager.SignInAsync(user, isPersistent: false);

            return true;
        }


        public async Task Logout()
        {
            await _signInManager.SignOutAsync();
        }
    }
}
