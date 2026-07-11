using Microsoft.AspNetCore.Mvc;
using WebApplication1.DBContext;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    [Route("/")]
    public class HomeController : Controller
    {
        private readonly IConfiguration _configuration;
        private readonly AppDbContext _dBContext;

        public HomeController(IConfiguration configuration, AppDbContext appDbContext)
        {
            _configuration = configuration;
            _dBContext = appDbContext;
        }

        [HttpGet("/")]
        public IActionResult Index()
        {
            ViewBag.MyKey = _configuration["MyKey"];
            //throw new InvalidOperationException();
            return View();
        }

        [HttpPost("/")]
        public async Task<IActionResult> Add(Person person)
        {

            Person personToAdd = new Person()
            {
                name = "Banhi",
                email = "banhi@mail.com"
            };
            
            await _dBContext.Persons.AddAsync(personToAdd);
            await _dBContext.SaveChangesAsync();

            return View("Index");
        }
    }
}
