using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebApplication1.Models;
using WebApplication1.Services;

namespace WebApplication1.Controllers
{
    [Route("[controller]")]
    public class AttendeeController : Controller
    {
        private readonly ITableStorageService _iTableStorageService;

        public AttendeeController(ITableStorageService tableStorageService)
        {
            _iTableStorageService = tableStorageService;
        }

        [Route("[action]")]
        // GET: HomeController1
        public async Task<ActionResult> Index()
        {
            List<AttendeeEntity> data = await _iTableStorageService.GetAttendeees();
            return View(data);
        }

        // GET: HomeController1/Details/5
        [Route("[action]")]
        public async Task<ActionResult> Details(string industry, string id)
        {
            var attendee = await _iTableStorageService.GetAttendeee(industry, id);
            List<AttendeeEntity> data = new List<AttendeeEntity>() { attendee };
            return View("Index",data);
        }

        // GET: HomeController1/Create
        [Route("[action]")]
        public ActionResult Create()
        {
            return View();
        }

        // POST: HomeController1/Create
        [Route("[action]")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(AttendeeEntity attendeeEntity)
        {
            try
            {
                attendeeEntity.PartitionKey = attendeeEntity.Industry;
                attendeeEntity.RowKey = Guid.NewGuid().ToString();

                await _iTableStorageService.UpsertAttendeee(attendeeEntity);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: HomeController1/Edit/5
        [Route("[action]")]
        public async Task<ActionResult> Edit(string industry, string id)
        {
            var attendee = await _iTableStorageService.GetAttendeee(industry, id);
            List<AttendeeEntity> data = new List<AttendeeEntity>() { attendee };
            return View("Index", data);
        }


        // POST: HomeController1/Edit/5
        [Route("[action]")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(string id, AttendeeEntity attendeeEntity)
        {
            try
            {
                attendeeEntity.PartitionKey = attendeeEntity.Industry;

                await _iTableStorageService.UpsertAttendeee(attendeeEntity);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // POST: HomeController1/Delete/5
        [Route("[action]")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Delete(string id, string industry)
        {
            try
            {
                await _iTableStorageService.DeleteAttendeee(industry,id);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
