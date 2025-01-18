using Microsoft.AspNetCore.Mvc;

namespace Store_Core.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Dashboard()
        {
            return View();
        }
    }
}
