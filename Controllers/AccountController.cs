using Microsoft.AspNetCore.Mvc;
using INF4001N_1814748_NVSAAY001_2024.ViewModels;

namespace INF4001N_1814748_NVSAAY001_2024.Controllers
{
    public class AccountController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login(LoginViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            return RedirectToAction("Index", "Home");
        }
    }
}
