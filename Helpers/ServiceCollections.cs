using Aducati.Azure.TableStorage.Repository;
using Educati.Azure.Function.Api.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Educati.Azure.Function.Api.Helpers
{
    public class ServiceCollections
    {
        public static void RegisterServices(IServiceCollection services)
        {
            services.AddScoped<IUserService, UserService>();
            services.AddSingleton<ITableStorage, AzureTableStorage>(s => new AzureTableStorage(FunctionConfigs.TableStorageConnstionString));
            
        }
    }
}
