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
    using NSwag.Annotations;
    using System.IO;
    using System.Threading.Tasks;
    using System.Web.Http;

    /// <summary>
    /// Defines the <see cref="TenantFunction" />.
    /// </summary>
    public class TenantFunction : AuthenticationFilter
    {
        /// <summary>
        /// Defines the _tenantService.
        /// </summary>
        private readonly ITenantService _tenantService;

        /// <summary>
        /// Initializes a new instance of the <see cref="TenantFunction"/> class.
        /// </summary>
        /// <param name="tenantService">The tenantService<see cref="ITenantService"/>.</param>
        public TenantFunction(ITenantService tenantService)
        {
            _tenantService = tenantService;
        }

        /// <summary>
        /// The TenantCreate.
        /// </summary>
        /// <param name="request">The request<see cref="HttpRequest"/>.</param>
        /// <returns>The <see cref="Task{IActionResult}"/>.</returns>
        [FunctionName("TenantCreate")]
        public async Task<IActionResult> TenantCreate(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post",  Route = "tenant/create")]
            [RequestBodyType(typeof(Tenant), "Create tenant request")] HttpRequest request)
        {

            string requestBody = await new StreamReader(request.Body).ReadToEndAsync();
            Tenant requestData = JsonConvert.DeserializeObject<Tenant>(requestBody);

            try
            {
                await _tenantService.CreateUpdate(requestData);
            }
            catch (HttpResponseException ex)
            {

                return new BadRequestObjectResult(ex);

            }
            return new OkObjectResult(new { message = $"Tenant {requestData.Name} create successfully." });
        }

        [FunctionName("TenantData")]
        public async Task<IActionResult> TenantData([HttpTrigger(AuthorizationLevel.Anonymous, "post",  Route = "tenant/data")]
            [RequestBodyType(typeof(DataRequest), "Get tenant data")] HttpRequest request)
        {
            var validateStatus = base.AuthorizationStatus(request);
            if (validateStatus != System.Net.HttpStatusCode.Accepted)
            {
                return new BadRequestObjectResult(validateStatus);
            }

            string requestBody = await new StreamReader(request.Body).ReadToEndAsync();
            DataRequest requestData = JsonConvert.DeserializeObject<DataRequest>(requestBody);

            var response = await _tenantService.GetDataResponse(requestData);

            return new OkObjectResult(response);
        }

        [FunctionName("TenantList")]
        [OpenApiOperation("List", "Tenant")]
        public async Task<IActionResult> TeacherList(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "tenants")] HttpRequest req)
        {
            var validateStatus = base.AuthorizationStatus(req);
            if (validateStatus != System.Net.HttpStatusCode.Accepted)
            {
                return new BadRequestObjectResult(validateStatus);
            }

            var response = await _tenantService.GetAll();

            return new OkObjectResult(response);
        }

    }
}
