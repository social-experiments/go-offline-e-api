using Educati.Helpers.Attributes;
using Educati.Helpers.Enums;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.IO;
using System.Threading.Tasks;

namespace Educati.Functions
{
    public  class SchoolFunction: AuthenticationFilter
    {
        [FunctionName("School")]
        public  async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", Route = null)] HttpRequest req,
            ILogger log)
        {

            if (base.accessTokenStatus == AccessTokenStatus.Valid)
            {
                return new OkResult();
            }
            else
            {
                return new UnauthorizedResult();
            }

        }
    }
}
