using Microsoft.AspNetCore.Mvc;

namespace FinalProject_MVC_With_Identity.Controllers
{
    public class AdminController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
