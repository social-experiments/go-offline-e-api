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

namespace goOfflineE.Functions
{
    public class ProfileFunction : AuthenticationFilter
    {
        private readonly IProfileService _profileService;

        public ProfileFunction(IProfileService profileService)
        {
            _profileService = profileService;
        }

        [FunctionName("Profile")]
        public async Task<IActionResult> Profile(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post",  Route = "user/profile")]
            [RequestBodyType(typeof(ProfileUpdateRequest), "User profile update request")] HttpRequest request)
        {
            var validateStatus = base.AuthorizationStatus(request);
            if (validateStatus != HttpStatusCode.Accepted)
            {
                return new BadRequestObjectResult(validateStatus);
            }

            string requestBody = await new StreamReader(request.Body).ReadToEndAsync();
            ProfileUpdateRequest requestData = JsonConvert.DeserializeObject<ProfileUpdateRequest>(requestBody);

            try
            {
                await _profileService.UpdateProfile(requestData);
            }
            catch (HttpResponseException ex)
            {

                return new BadRequestObjectResult(ex);

            }
            return new OkObjectResult(new { message = "Profile update successfully." });
        }

        [FunctionName("UserSignup")]
        public async Task<IActionResult> Register(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post",  Route = "signup")]
            [RequestBodyType(typeof(RegisterRequest), "New user signup request")] HttpRequest request)
        {
            string requestBody = await new StreamReader(request.Body).ReadToEndAsync();
            RegisterRequest requestData = JsonConvert.DeserializeObject<RegisterRequest>(requestBody);

            await _profileService.Register(requestData);

            return new OkObjectResult(new { message = "Registration successful." });
        }
    }
}
