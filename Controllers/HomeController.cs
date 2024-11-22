using INF4001N_1814748_NVSAAY001_2024.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace INF4001N_1814748_NVSAAY001_2024.Controllers
{
    public class HomeController : Controller
    {
        //Logger instance for logging application events and errors
        private readonly ILogger<HomeController> _logger;

        //Constructor to inject the ILogger service
        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        //Action to render the Home/Index view
        public IActionResult Index()
        {
            return View();
        }

        //Action to handle and display error information
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            //Creates a new ErrorViewModel with the current request's trace identifier
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
