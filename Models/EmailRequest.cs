namespace goOfflineE.Models
{
    using MimeKit;
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// Defines the <see cref="EmailRequest" />.
    /// </summary>
    public class EmailRequest
    {
        /// <summary>
        /// Gets or sets the Subject.
        /// </summary>
        public string Subject { get; set; }

        /// <summary>
        /// Gets or sets the To.
        /// </summary>
        public List<string> To { get; set; }

        /// <summary>
        /// Gets or sets the FromEmail.
        /// </summary>
        public string FromEmail { get; set; }

        /// <summary>
        /// Gets or sets the FromName.
        /// </summary>
        public string FromName { get; set; }

        /// <summary>
        /// Gets or sets the HtmlContent.
        /// </summary>
        public string HtmlContent { get; set; }
    }

    /// <summary>
    /// Defines the <see cref="Message" />.
    /// </summary>
    public class Message
    {
        /// <summary>
        /// Gets or sets the To.
        /// </summary>
        public List<MailboxAddress> To { get; set; }

        /// <summary>
        /// Gets or sets the FromEmail.
        /// </summary>
        public string FromEmail { get; set; }

        /// <summary>
        /// Gets or sets the FromName.
        /// </summary>
        public string FromName { get; set; }

        /// <summary>
        /// Gets or sets the Subject.
        /// </summary>
        public string Subject { get; set; }

        /// <summary>
        /// Gets or sets the Content.
        /// </summary>
        public string Content { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="Message"/> class.
        /// </summary>
        /// <param name="emailRequest">The emailRequest<see cref="EmailRequest"/>.</param>
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
