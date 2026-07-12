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
        private readonly IBlobStorageService _iBlobStorageService;

        public AttendeeController(ITableStorageService tableStorageService, IBlobStorageService iBlobStorageService)
        {
            _iTableStorageService = tableStorageService;
            _iBlobStorageService = iBlobStorageService;
        }

        [Route("[action]")]
        // GET: HomeController1
        public async Task<ActionResult> Index()
        {
            List<AttendeeEntity> data = await _iTableStorageService.GetAttendeees();
            foreach(var attendee in data)
            {
                if (!string.IsNullOrEmpty(attendee.ProfileImage))
                {
                    attendee.ProfileImage = await _iBlobStorageService.GetBlobUrl(attendee.ProfileImage);   //guid is sent
                }
            }
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
        public async Task<ActionResult> Create(AttendeeEntity attendeeEntity, IFormFile formFile)
        {
            try
            {
                attendeeEntity.PartitionKey = attendeeEntity.Industry;
                attendeeEntity.RowKey = Guid.NewGuid().ToString();

                //if image exists
                if(formFile != null && formFile.Length > 0)
                {
                    var fileName = attendeeEntity.RowKey;
                    //final guid.extension is saved as blob name in azure blob storage and database
                    attendeeEntity.ProfileImage = await _iBlobStorageService.UploadBlob(formFile, fileName);
                }
                else
                {
                    attendeeEntity.ProfileImage = "defaultprofile.png"; // Set default image if no image is uploaded
                }

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
                //fetches the data for profile image
                var attendeeData = await _iTableStorageService.GetAttendeee(industry, id);
                await _iTableStorageService.DeleteAttendeee(industry,id);
                await  _iBlobStorageService.RemoveBlob(attendeeData.ProfileImage);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
