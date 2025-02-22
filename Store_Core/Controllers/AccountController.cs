using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

 using Microsoft.Extensions.Localization;
using Store.Core.Application.Sarvice;
using Store.Core.Application.Sarvice.ISarvice;
using Store.Infrastructure.Models;
using Store_Core.Resources;
using Store_Core.ViewModels;
using System.Web;

namespace Store_Core.Controllers
{
    public class AccountController : Controller
    {
        private readonly AuthService _authService;
        private readonly UserManager<ApplicationUser> _userManager;  
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IStringLocalizer<SharedResource> _localizer;
        private readonly IWebHostEnvironment _env;
        private readonly IEmailService _emailService;



        public AccountController(AuthService authService, UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, IStringLocalizer<SharedResource> localizer, IWebHostEnvironment env, IEmailService emailService)
        {
            _authService = authService;
            _userManager = userManager;
            _signInManager = signInManager;
            _localizer = localizer;
            _env = env;
            _emailService = emailService;



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

            var result = await _authService.RegisterUser(model.Email, model.Password, model.FullName);

            if (!result.Succeeded)   
            {
                foreach (var error in result.Errors)
                {
                    string errorMessage = error.Description;

                     if (error.Code.Contains("PasswordRequiresNonAlphanumeric"))
                        errorMessage = "يجب أن تحتوي كلمة المرور على رمز خاص مثل !@#$%^&*";
                    else if (error.Code.Contains("PasswordRequiresLower"))
                        errorMessage = "يجب أن تحتوي كلمة المرور على حرف صغير على الأقل (a-z).";
                    else if (error.Code.Contains("PasswordRequiresUpper"))
                        errorMessage = "يجب أن تحتوي كلمة المرور على حرف كبير على الأقل (A-Z).";
                    else if (error.Code.Contains("PasswordRequiresDigit"))
                        errorMessage = "يجب أن تحتوي كلمة المرور على رقم واحد على الأقل (0-9).";
                    else if (error.Code.Contains("PasswordTooShort"))
                        errorMessage = "كلمة المرور قصيرة جدًا. يجب أن تتكون من 8 أحرف على الأقل.";
                    else if (error.Code.Contains("DuplicateUserName") || error.Code.Contains("DuplicateEmail"))
                        errorMessage = "البريد الإلكتروني مُستخدم بالفعل. يرجى اختيار بريد آخر.";

                    ModelState.AddModelError(string.Empty, errorMessage);
                }
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

        [HttpGet]
        public IActionResult ForgotPassword()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ForgotPassword(ForgetPasswordViewModel model)
        {
            if (!ModelState.IsValid) return View(model);

            if (string.IsNullOrEmpty(model.Email))
            {
                ModelState.AddModelError("Email", "Email address is required.");
                return View(model);
            }

            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null)
            {
                return View("ForgotPasswordConfirmation");
            }

            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            var resetLink = Url.Action("ResetPassword", "Account", new { email = model.Email, token = token }, Request.Scheme);

            string emailBody = $"<h3>إعادة تعيين كلمة المرور</h3><p>انقر على الرابط أدناه لإعادة تعيين كلمة المرور:</p><a href='{resetLink}'>إعادة تعيين كلمة المرور</a>";
            await _emailService.SendEmailAsync(user.Email, "إعادة تعيين كلمة المرور", emailBody);

            return View("ForgotPasswordConfirmation");
        }

        [HttpGet]
        public IActionResult ResetPassword(string email, string token)
        {
            return View(new ResetPasswordViewModel { Email = email, Token = token });
        }

        [HttpPost]
        public async Task<IActionResult> ResetPassword(ResetPasswordViewModel model)
        {
            if (!ModelState.IsValid) return View(model);

             var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null)
            {
                ModelState.AddModelError("", "لم يتم العثور على مستخدم بهذا البريد الإلكتروني.");
                return View(model);
            }

             var result = await _userManager.ResetPasswordAsync(user, model.Token, model.NewPassword);
            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                    ModelState.AddModelError("", error.Description);

                return View(model);
            }

            return View("ResetPasswordConfirmation");
        }


    }
}
