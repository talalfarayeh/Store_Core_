using Microsoft.AspNetCore.Mvc;
using Store.Core.Application.Sarvice;
using Store_Core.ViewModels;
using System.Threading.Tasks;

namespace Store_Core.Controllers
{
    public class AccountController : Controller
    {
        private readonly AuthService _authService;

        public AccountController(AuthService authService)
        {
            _authService = authService;
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
            if (!result)
            {
                ModelState.AddModelError("", "An error occurred during registration. The email may already be in use.");
                return View(model);
            }

            return RedirectToAction("Index", "Home");  
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
    }
}
