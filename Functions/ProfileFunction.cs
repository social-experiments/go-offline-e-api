using AzureFunctions.Extensions.Swashbuckle.Attribute;
using Educati.Azure.Function.Api.Helpers.Attributes;
using Educati.Azure.Function.Api.Models;
using Educati.Azure.Function.Api.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Newtonsoft.Json;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace Educati.Azure.Function.Api.Functions
{
    public class ProfileFunction : AuthenticationFilter
    {
        private readonly IProfileService _profileService;

        public ProfileFunction(IProfileService profileService)
        {
            _profileService = profileService;
        }

        [FunctionName("Profile")]
        public async Task<HttpResponseMessage> Profile(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post",  Route = "user/profile")]
            [RequestBodyType(typeof(ProfileUpdateRequest), "User profile update request")] HttpRequest request)
        {
            string requestBody = await new StreamReader(request.Body).ReadToEndAsync();
            ProfileUpdateRequest requestData = JsonConvert.DeserializeObject<ProfileUpdateRequest>(requestBody);

            try
            {
                await _profileService.UpdateProfile(requestData);
            }
            catch (HttpResponseException ex)
            {
                return new HttpResponseMessage(ex.Response.StatusCode)
                {
                    Content = new StringContent(string.Format(ex.Message)),
                };

            }
            return new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(string.Format("Profile update sucessfully!")),
            };
        }
    }
}
