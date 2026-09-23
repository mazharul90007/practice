using System.Runtime.Versioning;
using Microsoft.AspNetCore.Mvc;
using view_practice.Models;

namespace view_practice.Controllers
{
    public class HomeController : Controller
    {
        [Route("home")]
        public IActionResult Index()
        {
            ViewData["appTitle"] = "View Practice";

            ViewData["person"] = new Person()
            {
                name = "Mazharul Islam",
                dateOfBirth = null,
                personGender = Gender.Male
            };

            var people = new List<Person>
            {
                new Person()
                {
                    name = "Mazharul Islam",
                    dateOfBirth = null,
                    personGender = Gender.Male
                },

                new Person()
                {
                    name = "Nusrat Jahan",
                    dateOfBirth = new DateTime(2000, 05, 05),
                    personGender = Gender.Female
                },

                new Person()
                {
                    name = "Rakib Hasan",
                    dateOfBirth = null,
                    personGender = Gender.Male
                }
            };

            // ViewData["people"] = people;
            // ViewBag.people = people;
            return View("Index", people);
        }
    }
}
