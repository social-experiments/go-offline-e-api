using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace goOfflineE.Helpers
{
    public class SwaggerUIConfigurations
    {
        public static void SwaggerUIConfig(SwaggerGenOptions swaggerGenOption)
        {

            //This is to generate the Default UI of Swagger Documentation  
            swaggerGenOption.SwaggerDoc("v2", new OpenApiInfo
            {
                Version = "v2",
                Title = "goOfflineE",
                Description = "goOfflineE Azure Function API"
            });
            // To Enable authorization using Swagger (JWT) 
            swaggerGenOption.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
            {
                Name = "Authorization",
                Type = SecuritySchemeType.ApiKey,
                Scheme = "Bearer",
                BearerFormat = "JWT",
                In = ParameterLocation.Header,
                Description = "JWT Authorization header using the Bearer scheme. " +
                              "\r\n\r\n Enter 'Bearer' [space] and then your token in the text input below.\r\n\r\nExample: \"Bearer \"" + SettingConfigurations.IssuerToken,
            });
            swaggerGenOption.AddSecurityRequirement(
                new OpenApiSecurityRequirement
                    {
                        {
                              new OpenApiSecurityScheme
                                {
                                    Reference = new OpenApiReference
                                    {
                                        Type = ReferenceType.SecurityScheme,
                                        Id = "Bearer"
                                    }
                                },
                                new string[] {}
                        }
                    }
                );
        }
    }
}
