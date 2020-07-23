
using Aducati.Azure.TableStorage.Repository;
using AutoMapper;
using AzureFunctions.Extensions.Swashbuckle;
using Educati;
using Educati.Azure.Function.Api.Helpers;
using Educati.Azure.Function.Api.Services;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
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
            
            //This is to generate the Default UI of Swagger Documentation  
            builder.AddSwashBuckle(Assembly.GetExecutingAssembly(), option =>
            {
                option.ConfigureSwaggerGen = new System.Action<Swashbuckle.AspNetCore.SwaggerGen.SwaggerGenOptions>(swagger =>
                {
                    SwaggerUIConfigurations.SwaggerUIConfig(swagger);
                });
            });

            ConfigureServices(builder.Services);
        }

        private void ConfigureServices(IServiceCollection services)
        {
            services.AddAutoMapper(typeof(Startup));
            services.AddTransient<IAccountService, AccountService>();
            services.AddTransient<ISchoolService, SchoolService>();
            services.AddSingleton<ITableStorage, AzureTableStorage>(s => new AzureTableStorage(FunctionConfigs.TableStorageConnstionString));
        }
    }

}
