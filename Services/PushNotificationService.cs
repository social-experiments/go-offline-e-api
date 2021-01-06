namespace goOfflineE.Services
{
    using goOfflineE.Common.Constants;
    using goOfflineE.Helpers;
    using goOfflineE.Models;
    using Microsoft.Extensions.Logging;
    using Newtonsoft.Json;
    using System;
    using System.Net.Http;
    using System.Text;
    using System.Threading.Tasks;

    /// <summary>
    /// Defines the <see cref="PushNotificationService" />.
    /// </summary>
    public class PushNotificationService : IPushNotificationService
    {
       
        /// <summary>
        /// Initializes a new instance of the <see cref="PushNotificationService"/> class.
        /// </summary>
        /// <param name="log">The log<see cref="ILogger"/>.</param>
        public PushNotificationService()
        {
        }

        /// <summary>
        /// The SendAsync.
        /// </summary>
        /// <param name="notification">The notification<see cref="PushNotification"/>.</param>
        /// <returns>The <see cref="Task{bool}"/>.</returns>
        public async Task<bool> SendAsync(PushNotification notification)
        {
             return await NotifyAsync(notification);
        }

        /// <summary>
        /// The NotifyAsync.
        /// </summary>
        /// <param name="notification">The notification<see cref="PushNotification"/>.</param>
        /// <returns>The <see cref="Task{bool}"/>.</returns>
        private async Task<bool> NotifyAsync(PushNotification notification)
        {
            try
            {
               
                // Get the sender id from FCM console
                var senderId = string.Format("id={0}", SettingConfigurations.PushNotificationSenderId);

                var data = new
                {
                    to = notification.RecipientDeviceToken, // Recipient device token
                    notification = new { title = notification.Title, body = notification.Body }
                };

                // Using Newtonsoft.Json
                var jsonBody = JsonConvert.SerializeObject(data);

                using (var httpRequest = new HttpRequestMessage(HttpMethod.Post, SettingConfigurations.PushNotificationApi))
                {
                    httpRequest.Headers.TryAddWithoutValidation("Authorization", SettingConfigurations.PushNotificationServerKey);
                    httpRequest.Headers.TryAddWithoutValidation("Sender", senderId);
                    httpRequest.Content = new StringContent(jsonBody, Encoding.UTF8, "application/json");

                    using (var httpClient = new HttpClient())
                    {
                        var result = await httpClient.SendAsync(httpRequest);

                        if (result.IsSuccessStatusCode)
                        {
                            return true;
                        }
                        else
                        {
                            // Use result.StatusCode to handle failure
                            // Your custom error handler here
                            throw new AppException("Error sending notification. Status Code: ", result.StatusCode);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new AppException("Exception thrown in Notify Service: ", ex.InnerException);

            }
        }
    }
}
