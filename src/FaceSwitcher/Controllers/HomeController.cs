using Microsoft.AspNetCore.Mvc;

namespace FaceSwitcher.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
