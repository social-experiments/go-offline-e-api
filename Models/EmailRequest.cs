using MimeKit;
using System.Collections.Generic;
using System.Linq;

namespace Educati.Azure.Function.Api.Models
{
    public class EmailRequest
    {
        public string Subject { get; set; }
        public List<string> To { get; set; }
        public string FromEmail { get; set; }
        public string FromName { get; set; }
        public string HtmlContent { get; set; }
    }

    public class Message
    {
        public List<MailboxAddress> To { get; set; }
        public string FromEmail { get; set; }
        public string FromName { get; set; }

        public string Subject { get; set; }
        public string Content { get; set; }

        public Message(EmailRequest emailRequest)
        {
            To = new List<MailboxAddress>();

            To.AddRange(emailRequest.To.Select(x => new MailboxAddress(x)));
            Subject = emailRequest.Subject;
            Content = emailRequest.HtmlContent;
            FromEmail = emailRequest.FromEmail;
        }
    }
}
