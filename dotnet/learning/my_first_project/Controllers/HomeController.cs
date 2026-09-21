using Microsoft.AspNetCore.Mvc;
using my_first_project.Models;

namespace my_first_project.Controllers
{
    [Controller]
    [Route("home")]
    public class HomeController: Controller
    {
        [Route("contact")]
        public string contact()
        {
            return "hello from contact page";
        }

        [Route("file-download")]
        public VirtualFileResult FileDownload()
        {
            return File("/ana_soft.jpg", "image/jpeg");
        }

        [Route("file-show")]
        public IActionResult FileDownload2()
        {
            return PhysicalFile(@"/home/mazharul-islam/Downloads/icon.png", "image/jpeg");
        }
    }
}