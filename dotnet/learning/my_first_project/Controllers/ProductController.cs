using Microsoft.AspNetCore.Mvc;
using my_first_project.Models;

namespace my_first_project.Controllers
{
    [Route("product")]
    public class ProductController : Controller
    {

        [Route("book/{bookid?}")] //route parameter 3
        public IActionResult Index([FromRoute]int? bookid, Book book)
        {

            if(bookid.HasValue == false)
            {
                return BadRequest("Book id is not supplied or empty");
            }


            if (bookid <= 0)
            {
                return Content("Book id can't be less than or equal to zero");
            }

            if (bookid > 1000)
            {
                return Content("Book id can't be greater than thousand");
            }


            return Content($"Book id: {bookid}, author's book: {book}", "text/plain");
        }
    }
}