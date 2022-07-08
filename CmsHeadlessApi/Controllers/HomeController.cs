using Microsoft.AspNetCore.Mvc;

namespace CmsHeadlessApi.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
