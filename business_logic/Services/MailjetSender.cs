using business_logic.Helpers;
using Mailjet.Client;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json.Linq;
using Mailjet.Client.Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace business_logic.Services
{
    public class MailjetSender : IEmailSender
    {
        private readonly IConfiguration _configuration;
        public MailjetSender(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            MailjetSettings? settings = _configuration.GetSection(nameof(MailjetSettings)).Get<MailjetSettings>();
            if (settings == null) throw new ArgumentNullException(nameof(settings));

            MailjetClient client = new MailjetClient(settings.ApiKey, settings.ApiSecret);
            MailjetRequest request = new MailjetRequest
            {
                Resource = Send.Resource,
            }
               .Property(Send.FromEmail, "onyxsupport@gmail.com")
               .Property(Send.FromName, "Onyx")
               .Property(Send.Subject, subject)
               .Property(Send.HtmlPart, htmlMessage)
               .Property(Send.Recipients, new JArray {
                    new JObject {
                        {"Email", email}
                    }
               });

            await client.PostAsync(request);
        }
    }
}
