namespace goOfflineE.Functions
{
    using AzureFunctions.Extensions.Swashbuckle.Attribute;
    using goOfflineE.Helpers.Attributes;
    using goOfflineE.Models;
    using goOfflineE.Services;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Azure.WebJobs;
    using Microsoft.Azure.WebJobs.Extensions.Http;
    using Newtonsoft.Json;
    using System.IO;
    using System.Net;
    using System.Threading.Tasks;
    using System.Web.Http;

    /// <summary>
    /// Defines the <see cref="PushNotificationFunction" />.
    /// </summary>
    public class PushNotificationFunction : AuthenticationFilter
    {
        /// <summary>
        /// Defines the _notification.
        /// </summary>
        private IPushNotificationService _notification;

        /// <summary>
        /// Initializes a new instance of the <see cref="PushNotificationFunction"/> class.
        /// </summary>
        /// <param name="notification">The notification<see cref="IPushNotificationService"/>.</param>
        public PushNotificationFunction(IPushNotificationService notification)
        {
            this._notification = notification;
        }

        /// <summary>
        /// The Run.
        /// </summary>
        /// <param name="request">The request<see cref="HttpRequest"/>.</param>
        /// <returns>The <see cref="Task{IActionResult}"/>.</returns>
        [FunctionName("PushNotificationFunction")]
        public async Task<IActionResult> SendPushNotification(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post",  Route = "push/notification")]
            [RequestBodyType(typeof(PushNotification), "Send push notification")] HttpRequest request)
        {

            var validateStatus = base.AuthorizationStatus(request);
            if (validateStatus != HttpStatusCode.Accepted)
            {
                return new BadRequestObjectResult(validateStatus);
            }

            string requestBody = await new StreamReader(request.Body).ReadToEndAsync();
            PushNotification requestData = JsonConvert.DeserializeObject<PushNotification>(requestBody);

            try
            {
                await _notification.SendAsync(requestData);
            }
            catch (HttpResponseException ex)
            {

                return new BadRequestObjectResult(ex);

            }
            return new OkObjectResult(new { message = "Push notification send successfully." });
        }
    }
}
