using Microsoft.AspNetCore.Mvc;

namespace XeniaWebServices.Controllers
{
    public class LeaderboardsController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
