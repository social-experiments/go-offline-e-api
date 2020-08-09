using Educati.Azure.Function.Api.Models;
using SendGrid;
using SendGrid.Helpers.Mail;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;

namespace Educati.Azure.Function.Api.Services
{
    public class EmailService : IEmailService
    {
        public async Task SendAsync(EmailRequest emailRequest)
        {
            await configSendGridasync(emailRequest);
        }

        private async Task configSendGridasync(EmailRequest message)
        {
            var apiKey = Environment.GetEnvironmentVariable("SENDGRID_API_KEY");
            var fromEmail = Environment.GetEnvironmentVariable("FROM_EMAIL");
            var fromEmailName = Environment.GetEnvironmentVariable("FROM_EMAIL_NAME");

            var client = new SendGridClient(apiKey);
            var from = new EmailAddress(fromEmail, fromEmailName);
            var to = new EmailAddress(message.To, message.Name);

            var msg = MailHelper.CreateSingleEmail(from, to, message.Subject, message.PlainTextContent, message.HtmlContent);

            var response = await client.SendEmailAsync(msg).ConfigureAwait(false);

            var result = response.Body;

        }
    }
}
