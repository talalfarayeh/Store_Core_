using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Store.Core.Application.Sarvice;
using Store.Infrastructure.Models;
using Store_Core.Resources;
using Store_Core.ViewModels;
using System.Threading.Tasks;

namespace Store_Core.Controllers
{
    public class AccountController : Controller
    {
        private readonly AuthService _authService;
        private readonly UserManager<ApplicationUser> _userManager;  
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IStringLocalizer<SharedResource> _localizer;
        private readonly IWebHostEnvironment _env;


        public AccountController(AuthService authService, UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, IStringLocalizer<SharedResource> localizer, IWebHostEnvironment env)
        {
            _authService = authService;
            _userManager = userManager;
            _signInManager = signInManager;
            _localizer = localizer;
            _env = env;
            

        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (!ModelState.IsValid) return View(model);

            
            var result = await _authService.RegisterUser(model.Email, model.Password ,model.FullName );

            if (!result)
            {
                ModelState.AddModelError("", "حدث خطأ أثناء التسجيل. قد يكون البريد الإلكتروني مستخدمًا بالفعل.");
                return View(model);
            }

            return RedirectToAction("Dashboard", "Home");
        }


        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (!ModelState.IsValid) return View(model);

            var result = await _authService.LoginUser(model.Email, model.Password);
            if (!result)
            {
                ModelState.AddModelError("", "The email or password is incorrect");
                return View(model);
            }

            return RedirectToAction("Dashboard", "Home"); 
        }

        public async Task<IActionResult> Logout()
        {
            await _authService.Logout();
            return RedirectToAction("Login");
        }
        [HttpGet]
        public async Task<IActionResult> EditProfile()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound();
            }

            var model = new EditProfileViewModel
            {
                FullName = user.FullName
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> EditProfile(EditProfileViewModel model, IFormFile profilePicture)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.GetUserAsync(User);
                if (user == null)
                {
                    return NotFound();
                }

               
                user.FullName = model.FullName;

              
                if (profilePicture != null && profilePicture.Length > 0)
                {
                    
                    if (!string.IsNullOrEmpty(user.ProfilePicture) && user.ProfilePicture != "/uploads/default_profile.jpg")
                    {
                        string oldFilePath = Path.Combine(_env.WebRootPath, user.ProfilePicture.TrimStart('/'));
                        if (System.IO.File.Exists(oldFilePath))
                        {
                            System.IO.File.Delete(oldFilePath);
                        }
                    }
 
                    string uploadsFolder = Path.Combine(_env.WebRootPath, "uploads");
                    Directory.CreateDirectory(uploadsFolder);
                    string uniqueFileName = Guid.NewGuid().ToString() + "_" + profilePicture.FileName;
                    string filePath = Path.Combine(uploadsFolder, uniqueFileName);

                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        await profilePicture.CopyToAsync(fileStream);
                    }

                    user.ProfilePicture = "/uploads/" + uniqueFileName;
                }

                var result = await _userManager.UpdateAsync(user);
                if (result.Succeeded)
                {
                    TempData["SuccessMessage"] = "تم تحديث الملف الشخصي بنجاح.";
                }
                else
                {
                    TempData["ErrorMessage"] = "حدث خطأ أثناء تحديث الملف الشخصي.";
                }

                return RedirectToAction("EditProfile");
            }

            return View(model);
        }
        [HttpGet]
        public IActionResult ChangePassword()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ChangePassword(ChangePasswordViewModel model)
        {
             if (!ModelState.IsValid)
            {
                return View(model); 
            }

            try
            {
                 
                var user = await _userManager.GetUserAsync(User);


                if (user == null)
                {
                    TempData["ErrorMessage"] = _localizer["UserNotFound"];
                    return RedirectToAction("Login");
                }

                 var result = await _userManager.ChangePasswordAsync(user, model.CurrentPassword, model.NewPassword);

                 if (!result.Succeeded)
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);  
                    }
                    return View(model); 
                }

                

                 TempData["SuccessMessage"] = _localizer["ChangePasswordSuccess"];

                 return RedirectToAction("Profile");
            }
            catch (Exception ex)
            {
                 Console.WriteLine($"Error in ChangePassword: {ex.Message}");

                 TempData["ErrorMessage"] = _localizer["ChangePasswordError"];
                return View(model); 
            }
        }


    }
}
