using Educati.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Educati.Helpers
{
    public class ServiceCollections
    {
        public static void RegisterServices(IServiceCollection services)
        {
            services.AddScoped<IUserService, UserService>();
        }
    }
}
