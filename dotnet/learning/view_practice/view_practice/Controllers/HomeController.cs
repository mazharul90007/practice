using System.Runtime.Versioning;
using Microsoft.AspNetCore.Mvc;

namespace view_practice.Controllers
{
    public class HomeController : Controller
    {
        [Route("home")]
        public IActionResult Index()
        {
            return View();
        }
    }
}
