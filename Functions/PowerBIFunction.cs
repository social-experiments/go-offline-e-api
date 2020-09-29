namespace goOfflineE.Functions
{
    using goOfflineE.Helpers.Attributes;
    using goOfflineE.Services;
    using Microsoft.AspNetCore.Cors;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Azure.WebJobs;
    using Microsoft.Azure.WebJobs.Extensions.Http;
    using NSwag.Annotations;
    using System.Threading.Tasks;

    /// <summary>
    /// Defines the <see cref="PowerBIFunction" />.
    /// </summary>
    public class PowerBIFunction : AuthenticationFilter
    {
        /// <summary>
        /// Defines the _powerBIService.
        /// </summary>
        private readonly IPowerBIService _powerBIService;

        /// <summary>
        /// Initializes a new instance of the <see cref="PowerBIFunction"/> class.
        /// </summary>
        /// <param name="powerService">The powerService<see cref="IPowerBIService"/>.</param>
        public PowerBIFunction(IPowerBIService powerService)
        {
            _powerBIService = powerService;
        }

        /// <summary>
        /// The GetPowerBIAccessToken.
        /// </summary>
        /// <param name="req">The req<see cref="HttpRequest"/>.</param>
        /// <returns>The <see cref="Task{IActionResult}"/>.</returns>
        [FunctionName("PowerBIAccessToken")]
        [OpenApiOperation("List", "PowerBI")]
        [DisableCors]
        public async Task<IActionResult> GetPowerBIAccessToken(
           [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "powerbi/token")] HttpRequest req)
        {
            var validateStatus = base.AuthorizationStatus(req);
            if (validateStatus != System.Net.HttpStatusCode.Accepted)
            {
                return new BadRequestObjectResult(validateStatus);
            }

            var response = await _powerBIService.GetPowerBIAccessToken();

            return new OkObjectResult(response);
        }
    }
}
