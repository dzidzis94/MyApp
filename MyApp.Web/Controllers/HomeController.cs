using Microsoft.AspNetCore.Mvc;

namespace MyApp.Web.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Project");
            }
            return View();
        }

        public IActionResult Error()
        {
            return View();
        }
    }
}
