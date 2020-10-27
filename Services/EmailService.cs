namespace goOfflineE.Services
{
    using goOfflineE.Common.Constants;
    using goOfflineE.Models;
    using MailKit.Net.Smtp;
    using MimeKit;
    using System;
    using System.Threading.Tasks;

    /// <summary>
    /// Defines the <see cref="EmailService" />.
    /// </summary>
    public class EmailService : IEmailService
    {
        /// <summary>
        /// The SendAsync.
        /// </summary>
        /// <param name="emailRequest">The emailRequest<see cref="EmailRequest"/>.</param>
        /// <returns>The <see cref="Task"/>.</returns>
        public async Task SendAsync(EmailRequest emailRequest)
        {
            Message message = new Message(emailRequest);
            var emailMessage = CreateEmailMessage(message);

            await SendAsync(emailMessage);
        }

        /// <summary>
        /// The SendAsync.
        /// </summary>
        /// <param name="mailMessage">The mailMessage<see cref="MimeMessage"/>.</param>
        /// <returns>The <see cref="Task"/>.</returns>
        private async Task SendAsync(MimeMessage mailMessage)
        {
            using (var client = new SmtpClient())
            {
                try
                {
                    await client.ConnectAsync(SettingConfigurations.SMTPServer, SettingConfigurations.SMTPPort, true);
                    client.AuthenticationMechanisms.Remove("XOAUTH2");
                    await client.AuthenticateAsync(SettingConfigurations.SMTPUser, SettingConfigurations.SMTPPassword);

                    await client.SendAsync(mailMessage);
                }
                catch (Exception)
                {
                    //log an error message or throw an exception, or both.
                   
                }
                finally
                {
                    await client.DisconnectAsync(true);
                    client.Dispose();
                }
            }
        }

        /// <summary>
        /// The CreateEmailMessage.
        /// </summary>
        /// <param name="message">The message<see cref="Message"/>.</param>
        /// <returns>The <see cref="MimeMessage"/>.</returns>
        private MimeMessage CreateEmailMessage(Message message)
        {
            var emailMessage = new MimeMessage();
            if (!string.IsNullOrEmpty(message.FromEmail))
            {
                emailMessage.From.Add(new MailboxAddress(message.FromEmail));
            }
            else
            {
                emailMessage.From.Add(new MailboxAddress(SettingConfigurations.SMTPUser));
            }
            emailMessage.To.AddRange(message.To);
            emailMessage.Subject = message.Subject;
            message.Content += $"<p>Click here to login: <a href=${SettingConfigurations.WebSiteUrl}>${SettingConfigurations.WebSiteUrl}</a> </p>";
            emailMessage.Body = new TextPart(MimeKit.Text.TextFormat.Html) { Text = message.Content };

            return emailMessage;
        }
    }
}
