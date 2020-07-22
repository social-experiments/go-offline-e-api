using AzureFunctions.Extensions.Swashbuckle.Attribute;
using Educati.Azure.Function.Api.Models;
using Educati.Azure.Function.Api.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Newtonsoft.Json;
using System.IO;
using System.Threading.Tasks;

namespace Educati.Azure.Function.Api.Functions
{
    public class AccountFunction
    {
        private readonly IAccountService _accountService;
        public AccountFunction(IAccountService accountService)
        {
            _accountService = accountService;
        }

        [FunctionName("Login")]
        public async Task<IActionResult> Login(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post",  Route = "login")]
            [RequestBodyType(typeof(AuthenticateRequest), "User authentication request")] HttpRequest request)
        {
            string requestBody = await new StreamReader(request.Body).ReadToEndAsync();
            AuthenticateRequest requestData = JsonConvert.DeserializeObject<AuthenticateRequest>(requestBody);

            var response = await _accountService.Authenticate(requestData);

            return new OkObjectResult(response);
        }

        [FunctionName("Signup")]
        public async Task<IActionResult> Register(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post",  Route = "signup")]
            [RequestBodyType(typeof(RegisterRequest), "New user account request")] HttpRequest request)
        {
            string requestBody = await new StreamReader(request.Body).ReadToEndAsync();
            RegisterRequest requestData = JsonConvert.DeserializeObject<RegisterRequest>(requestBody);

            await _accountService.Register(requestData);

            return new OkObjectResult(new { message = "Registration successful." });
        }
    }
}
