using Microsoft.AspNetCore.Mvc;

namespace WebApplication1.Controllers
{
    [Route("/")]
    public class HomeController : Controller
    {
        private readonly IConfiguration _configuration;

        public HomeController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpGet("/")]
        public IActionResult Index()
        {
            //ViewBag.MyKey = _configuration["MyKey"];
            //throw new InvalidOperationException();
            return View();
        }
    }
}
