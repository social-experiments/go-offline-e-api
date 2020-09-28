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
    /// Defines the <see cref="ContentFunction" />.
    /// </summary>
    public class ContentFunction : AuthenticationFilter
    {
        /// <summary>
        /// Defines the _contentService.
        /// </summary>
        private readonly IContentService _contentService;

        /// <summary>
        /// Initializes a new instance of the <see cref="ContentFunction"/> class.
        /// </summary>
        /// <param name="contentService">The contentService<see cref="IContentService"/>.</param>
        public ContentFunction(IContentService contentService)
        {
            _contentService = contentService;
        }

        /// <summary>
        /// The Content Create Update.
        /// </summary>
        /// <param name="request">The request<see cref="HttpRequest"/>.</param>
        /// <returns>The <see cref="Task{IActionResult}"/>.</returns>
        [FunctionName("ContentCreateUpdate")]
        [OpenApiOperation("Create/Update", "Content")]
        public async Task<IActionResult> ContentCreateUpdate(
         [HttpTrigger(AuthorizationLevel.Anonymous, "post",  Route = "content/create")]
            [RequestBodyType(typeof(Content), "Create/update content")] HttpRequest request)
        {
            var validateStatus = base.AuthorizationStatus(request);
            if (validateStatus != System.Net.HttpStatusCode.Accepted)
            {
                return new BadRequestObjectResult(validateStatus);
            }

            string requestBody = await new StreamReader(request.Body).ReadToEndAsync();
            Content requestData = JsonConvert.DeserializeObject<Content>(requestBody);

            try
            {
                await _contentService.CreateUpdate(requestData);

            }
            catch (HttpResponseException ex)
            {
                return new ConflictObjectResult(ex.Response.Content);

            }
            return new OkObjectResult(new { message = "Create/update content successful." });
        }

        /// <summary>
        /// The Content List.
        /// </summary>
        /// <param name="req">The req<see cref="HttpRequest"/>.</param>
        /// <returns>The <see cref="Task{IActionResult}"/>.</returns>
        [FunctionName("ContentList")]
        [OpenApiOperation("List", "Content")]
        public async Task<IActionResult> ContentList(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "contents")] HttpRequest req)
        {
            var validateStatus = base.AuthorizationStatus(req);
            if (validateStatus != System.Net.HttpStatusCode.Accepted)
            {
                return new BadRequestObjectResult(validateStatus);
            }

            var response = await _contentService.GetAll();

            return new OkObjectResult(response);
        }

        /// <summary>
        /// The ContentDistribution.
        /// </summary>
        /// <param name="request">The request<see cref="HttpRequest"/>.</param>
        /// <returns>The <see cref="Task{IActionResult}"/>.</returns>
        [FunctionName("ContentDistribution")]
        [OpenApiOperation("Content/Distribution", "Content")]
        public async Task<IActionResult> ContentDistribution(
         [HttpTrigger(AuthorizationLevel.Anonymous, "post",  Route = "content/distribution")]
            [RequestBodyType(typeof(Distribution), "Content distribution")] HttpRequest request)
        {
            var validateStatus = base.AuthorizationStatus(request);
            if (validateStatus != System.Net.HttpStatusCode.Accepted)
            {
                return new BadRequestObjectResult(validateStatus);
            }

            string requestBody = await new StreamReader(request.Body).ReadToEndAsync();
            Distribution requestData = JsonConvert.DeserializeObject<Distribution>(requestBody);

            try
            {
                await _contentService.ContentDistribution(requestData);

            }
            catch (HttpResponseException ex)
            {
                return new ConflictObjectResult(ex.Response.Content);

            }
            return new OkObjectResult(new { message = "Content distribution successful." });
        }
    }
}
