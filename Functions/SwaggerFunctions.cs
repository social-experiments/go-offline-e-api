namespace goOfflineE.Functions
{
    using AzureFunctions.Extensions.Swashbuckle;
    using AzureFunctions.Extensions.Swashbuckle.Attribute;
    using Microsoft.Azure.WebJobs;
    using Microsoft.Azure.WebJobs.Extensions.Http;
    using System.Net.Http;
    using System.Threading.Tasks;

    /// <summary>
    /// Defines the <see cref="SwaggerFunctions" />.
    /// </summary>
    public static class SwaggerFunctions
    {
        /// <summary>
        /// The Swagger.
        /// </summary>
        /// <param name="req">The req<see cref="HttpRequestMessage"/>.</param>
        /// <param name="swashBuckleClient">The swashBuckleClient<see cref="ISwashBuckleClient"/>.</param>
        /// <returns>The <see cref="Task{HttpResponseMessage}"/>.</returns>
        [SwaggerIgnore]
        [FunctionName("Swagger")]
        public static Task<HttpResponseMessage> Swagger(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "swagger/json")]
            HttpRequestMessage req,
            [SwashBuckleClient] ISwashBuckleClient swashBuckleClient)
        {
            return Task.FromResult(swashBuckleClient.CreateSwaggerJsonDocumentResponse(req));
        }

        /// <summary>
        /// The SwaggerUi.
        /// </summary>
        /// <param name="req">The req<see cref="HttpRequestMessage"/>.</param>
        /// <param name="swashBuckleClient">The swashBuckleClient<see cref="ISwashBuckleClient"/>.</param>
        /// <returns>The <see cref="Task{HttpResponseMessage}"/>.</returns>
        [SwaggerIgnore]
        [FunctionName("SwaggerUi")]
        public static Task<HttpResponseMessage> SwaggerUi(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "swagger/ui")]
            HttpRequestMessage req,
            [SwashBuckleClient] ISwashBuckleClient swashBuckleClient)
        {
            return Task.FromResult(swashBuckleClient.CreateSwaggerUIResponse(req, "swagger/json"));
        }
    }
}
