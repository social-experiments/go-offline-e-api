using goOfflineE.Helpers.Attributes;
using goOfflineE.Services;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using NSwag.Annotations;
using System;
using System.Threading.Tasks;

namespace goOfflineE.Functions
{
    public class AzureBlobFunction: AuthenticationFilter
    {
        private readonly IAzureBlobService _blobService;
        public AzureBlobFunction(IAzureBlobService blobService)
        {
            _blobService = blobService;
        }

        [FunctionName("AzureSasURI")]
        [OpenApiOperation("List", "SAS")]
        public async Task<IActionResult> GetSASAccessToken(
          [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "azure/{container}/uri")] HttpRequest req, string container)
        {
            var validateStatus = base.AuthorizationStatus(req);
            if (validateStatus != System.Net.HttpStatusCode.Accepted)
            {
                return new BadRequestObjectResult(validateStatus);
            }

            var response = await _blobService.GetSasUri(container);

            return new OkObjectResult(response);
        }
    }
}
