
using Aducati.Azure.TableStorage.Repository;
using AutoMapper;
using AzureFunctions.Extensions.Swashbuckle;
using goOfflineE;
using goOfflineE.Helpers;
using goOfflineE.Services;
using Microsoft.Azure.CognitiveServices.Vision.Face;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;


[assembly: FunctionsStartup(typeof(Startup))]
namespace goOfflineE
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
            services.AddTransient<IProfileService, ProfileService>();
            services.AddTransient<ITeacherService, TeacherService>();
            services.AddTransient<IEmailService, EmailService>();
            services.AddTransient<IPowerBIService, PowerBIService>();
            services.AddTransient<IClassService, ClassService>();
            services.AddTransient<IStudentService, StudentService>();
            services.AddTransient<IAzureBlobService, AzureBlobService>();
            services.AddTransient<ICognitiveService, CognitiveService>();

            services.AddSingleton<ITableStorage, AzureTableStorage>(s => new AzureTableStorage(SettingConfigurations.AzureWebJobsStorage));
            services.AddSingleton<IFaceClient, FaceClient>(s => new FaceClient(new ApiKeyServiceClientCredentials(SettingConfigurations.CognitiveServiceKey),
            new System.Net.Http.DelegatingHandler[] { }));

        }
    }

}
