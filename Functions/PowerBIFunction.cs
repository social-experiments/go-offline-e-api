using goOfflineE.Helpers.Attributes;
using goOfflineE.Services;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using NSwag.Annotations;
using System.Threading.Tasks;

namespace goOfflineE.Functions
{
    public  class PowerBIFunction : AuthenticationFilter
    {
        private readonly IPowerBIService _powerBIService;
        public PowerBIFunction(IPowerBIService powerService)
        {
            _powerBIService = powerService;
        }

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
