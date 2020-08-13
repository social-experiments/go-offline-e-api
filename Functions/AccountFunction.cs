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

namespace goOfflineE.Functions
{
    public class AccountFunction : AuthenticationFilter
    {
        private readonly IAccountService _accountService;
        public AccountFunction(IAccountService accountService)
        {
            _accountService = accountService;
        }

        [FunctionName("AccountLogin")]
        public async Task<IActionResult> Login(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post",  Route = "login")]
            [RequestBodyType(typeof(AuthenticateRequest), "User authentication request")] HttpRequest request)
        {
            string requestBody = await new StreamReader(request.Body).ReadToEndAsync();
            AuthenticateRequest requestData = JsonConvert.DeserializeObject<AuthenticateRequest>(requestBody);

            var response = await _accountService.Authenticate(requestData);

            return new OkObjectResult(response);
        }

        [FunctionName("AccountSignup")]
        public async Task<IActionResult> Register(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post",  Route = "signup")]
            [RequestBodyType(typeof(RegisterRequest), "New user account request")] HttpRequest request)
        {
            string requestBody = await new StreamReader(request.Body).ReadToEndAsync();
            RegisterRequest requestData = JsonConvert.DeserializeObject<RegisterRequest>(requestBody);

            await _accountService.Register(requestData);

            return new OkObjectResult(new { message = "Registration successful." });
        }

        [FunctionName("AccountResetPassword")]
        public async Task<IActionResult> ResetPassword(
           [HttpTrigger(AuthorizationLevel.Anonymous, "post",  Route = "reset/password")]
            [RequestBodyType(typeof(ResetPasswordRequest), "Reset password request")] HttpRequest request)
        {
            var validateStatus = base.AuthorizationStatus(request);
            if (validateStatus != HttpStatusCode.Accepted)
            {
                return new BadRequestObjectResult(validateStatus);
            }
            string requestBody = await new StreamReader(request.Body).ReadToEndAsync();
            ResetPasswordRequest requestData = JsonConvert.DeserializeObject<ResetPasswordRequest>(requestBody);

            await _accountService.ResetPassword(requestData);

            return new OkObjectResult(new { message = "Reset password successfully." });
        }
    }
}
