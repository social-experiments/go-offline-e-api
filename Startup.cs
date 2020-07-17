
using Educati;
using Educati.Helpers;
using AzureFunctions.Extensions.Swashbuckle;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using System.Reflection;

[assembly: FunctionsStartup(typeof(Startup))]
namespace Educati
{
    // <summary>
    // Runs when the Azure Functions host starts.
    // </summary>
    public class Startup : FunctionsStartup
    {
        public override void Configure(IFunctionsHostBuilder builder)
        {
            // Register services
            ServiceCollections.RegisterServices(builder.Services);

            //This is to generate the Default UI of Swagger Documentation  
            builder.AddSwashBuckle(Assembly.GetExecutingAssembly(), option =>
            {
                option.ConfigureSwaggerGen = new System.Action<Swashbuckle.AspNetCore.SwaggerGen.SwaggerGenOptions>(swagger =>
                {
                    SwaggerUIConfigurations.SwaggerUIConfig(swagger);
                });
            });
          
        }
    }

}
