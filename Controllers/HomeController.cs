using Microsoft.AspNetCore.Mvc;

namespace XeniaWebServices.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
