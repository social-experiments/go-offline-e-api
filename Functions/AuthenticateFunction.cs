using Educati.Models;
using Educati.Services;
using AzureFunctions.Extensions.Swashbuckle.Attribute;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Newtonsoft.Json;
using System.IO;
using System.Threading.Tasks;

namespace Educati.Functions
{
    public class AuthenticateFunction
    {
        private readonly IUserService _userService;
        public AuthenticateFunction(IUserService userService)
        {
            _userService = userService;
        }

        [FunctionName("Login")]
        public async Task<IActionResult> Login(
            [HttpTrigger(AuthorizationLevel.Function, "post",  Route = "login")]
            [RequestBodyType(typeof(AuthenticateRequest), "user authentication request")] HttpRequest request)
        {
            string requestBody = await new StreamReader(request.Body).ReadToEndAsync();
            AuthenticateRequest requestData = JsonConvert.DeserializeObject<AuthenticateRequest>(requestBody);

            var response = _userService.Authenticate(requestData);

            if (response == null)
                return new BadRequestObjectResult(new { message = "Username or password is incorrect" });

            return new OkObjectResult(response);
        }
    }
}
