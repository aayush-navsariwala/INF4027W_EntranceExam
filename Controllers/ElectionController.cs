using Microsoft.AspNetCore.Mvc;

namespace INF4001N_1814748_NVSAAY001_2024.Controllers
{
    public class ElectionController : Controller
    {
        //Action to output the default view for the Elections page
        public IActionResult Index()
        {
            //Returns the default view associated with the Index action
            return View();
        }
    }
}
