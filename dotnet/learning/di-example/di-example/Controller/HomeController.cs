
using Microsoft.AspNetCore.Mvc;
using Services;
using ServiceContracts;

namespace DIExample.Controllers
{
    public class HomeController : Controller
    {
        private readonly ICitiesServices _citiesServices;
        private readonly IServiceScopeFactory _serviceScopeFactory;
        // Constructor
        public HomeController(ICitiesServices citiesServices, IServiceScopeFactory serviceScopeFactory)
        {
            _citiesServices = citiesServices;
            _serviceScopeFactory = serviceScopeFactory;
        }


        [Route("/")]
        public IActionResult Index()
        {

           List<string> cities = _citiesServices.GetCities();

            using (IServiceScope scope = _serviceScopeFactory.CreateAsyncScope())
            {
                //Inject CitiesService
               ICitiesServices citiesService = scope.ServiceProvider.GetRequiredService<ICitiesServices>();
                //DB work
            }//end of scope; it calls CitiesService.Dispose()

            return View(cities);
        }
    }
}