using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using BLL.Helpers;
using System.Net.Mail;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmailSenderController : Controller
    {
        public readonly IEmailSender emailSender;

        public EmailSenderController(IEmailSender emailSender)
        {
            this.emailSender = emailSender;
        }

        [HttpPost]
        public async Task<IActionResult> SendEmail([FromBody] SendMessageModel model)
        {
            await emailSender.SendEmailAsync(model.to, model.Subject, model.htmlMessage);
            return Ok();
        }
    }
}
