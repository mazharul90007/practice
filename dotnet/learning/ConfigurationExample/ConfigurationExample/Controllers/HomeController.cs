
using Microsoft.AspNetCore.Mvc;
using ConfigurationExample.Configuration;
using Microsoft.Extensions.Options;

namespace ConfigurationExample.Controllers
{
    public class HomeController(IOptions<WeatherApiOptions> configuration): Controller
    {
        [Route("/")]
        public IActionResult Index()
        {
            WeatherApiOptions? weatherApiOptions = configuration.Value;

            ViewBag.ClientId = weatherApiOptions?.ClientId;
            ViewBag.ClientSecret = weatherApiOptions?.ClientSecret;


            return View();
        }

        
    }
}
