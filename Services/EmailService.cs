using goOfflineE.Helpers;
using goOfflineE.Models;
using MailKit.Net.Smtp;
using MimeKit;
using System;
using System.Threading.Tasks;

namespace goOfflineE.Services
{
    public class EmailService : IEmailService
    {
        public async Task SendAsync(EmailRequest emailRequest)
        {
            Message message = new Message(emailRequest);
            var emailMessage = CreateEmailMessage(message);

            await SendAsync(emailMessage);
        }

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
                catch (Exception ex)
                {
                    //log an error message or throw an exception, or both.
                    throw;
                }
                finally
                {
                    await client.DisconnectAsync(true);
                    client.Dispose();
                }
            }
        }

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
            emailMessage.Body = new TextPart(MimeKit.Text.TextFormat.Html) { Text = message.Content };

            return emailMessage;
        }
    }
}
