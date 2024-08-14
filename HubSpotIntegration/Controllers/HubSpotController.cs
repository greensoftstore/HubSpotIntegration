using Microsoft.AspNetCore.Mvc;

namespace HubSpotIntegration.Controllers
{
    public class HubSpotController : Controller
    {
        private readonly HubSpotService _hubSpotService;

        public HubSpotController(HubSpotService hubSpotService)
        {
            _hubSpotService = hubSpotService;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult SendEmail()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> SendEmail(string emailId, string to, string from, string subject, string content, string sequenceId)
        {
            var emailResponse = await _hubSpotService.SendEmailAsync(emailId, to, from, subject, content);
            var sequenceResponse = await _hubSpotService.EnrollContactInSequenceAsync(to, sequenceId);

            ViewBag.EmailResponse = emailResponse;
            ViewBag.SequenceResponse = sequenceResponse;

            return View();
        }
    }
}
