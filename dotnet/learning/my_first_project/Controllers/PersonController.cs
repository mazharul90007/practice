using Microsoft.AspNetCore.Mvc;
using my_first_project.Models;

namespace my_first_project.Controllers
{
    [Route("user")]
    public class UserController : Controller
    {
        [Route("register")]
        public IActionResult Index([FromBody] Person person)
        {
            if (ModelState.IsValid == false)
            {
                string errors = string.Join("\n",  ModelState.Values.SelectMany(value => value.Errors).Select(err => err.ErrorMessage));

                return BadRequest(errors);
            }
            return Content($"{person}");
        }
    }
}