
using Microsoft.AspNetCore.Mvc;
using Models;

namespace Controllers
{
    public class HomeController : Controller
    {
        [Route("/")]
        public IActionResult Index()
        {
            return View();
        }

        [Route("about")]
        public IActionResult About()
        {
            return View();
        }

        [Route("programming")]
        public IActionResult ProgrammingLanguages()
        {
            ListModel listModel = new ListModel
            {
                ListTitle = "Programming Languages List",
                ListItems = new List<string>
            {
                "Python",
                "C#",
                "Go"
            }
            };

            return PartialView("_ListPartialView", listModel);
        }

    }
}