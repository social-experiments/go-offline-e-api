using Microsoft.AspNetCore.Http;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Primitives;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Educati.Azure.Function.Api.Helpers.Attributes
{

    /// <summary>
    /// Base class for authenticated service which checks the incoming JWT token.
    /// </summary>
    public abstract class AuthenticationFilter : IFunctionInvocationFilter
    {
        private const string AUTH_HEADER_NAME = "Authorization";
        private const string BEARER_PREFIX = "Bearer ";

        /// <summary>
        ///     Pre-execution filter.
        /// </summary>
        /// <remarks>
        ///     This mechanism can be used to extract the authentication information. 
        /// </remarks>
        public Task OnExecutingAsync(FunctionExecutingContext executingContext, CancellationToken cancellationToken)
        {
            var httpRequest = executingContext.Arguments.First().Value as HttpRequest;
            ValidateToken(httpRequest);
            return Task.CompletedTask;

        }

        /// <summary>
        ///     Post-execution filter.
        /// </summary>
        public Task OnExecutedAsync(FunctionExecutedContext executedContext, CancellationToken cancellationToken)
        {
            // Nothing.
            return Task.CompletedTask;
        }

        public HttpStatusCode AuthorizationStatus(HttpRequest request)
        {
            StringValues keys;
            if (!request.Headers.TryGetValue("AuthorizationStatus", out keys))
                return HttpStatusCode.ExpectationFailed;
            Int16 httpStatus = Convert.ToInt16(keys.First());
            return (HttpStatusCode)httpStatus;

        }

        private void ValidateToken(HttpRequest request)
        {
            try
            {
                var audience = SettingConfigurations.Audience;
                var issuer = SettingConfigurations.Issuer;
                var mySecurityKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(SettingConfigurations.IssuerToken));
                // Get the token from the header
                if (request != null &&
                    request.Headers.ContainsKey(AUTH_HEADER_NAME) &&
                    request.Headers[AUTH_HEADER_NAME].ToString().StartsWith(BEARER_PREFIX))
                {
                    var token = request.Headers[AUTH_HEADER_NAME].ToString().Substring(BEARER_PREFIX.Length);

                    // Create the parameters
                    var tokenParams = new TokenValidationParameters()
                    {
                        RequireSignedTokens = true,
                        ValidAudience = audience,
                        ValidateAudience = true,
                        ValidIssuer = issuer,
                        ValidateIssuer = true,
                        ValidateIssuerSigningKey = true,
                        ValidateLifetime = true,
                        IssuerSigningKey = mySecurityKey
                    };

                    // Validate the token
                    var handler = new JwtSecurityTokenHandler();
                    var result = handler.ValidateToken(token, tokenParams, out var securityToken);
                    if (result.HasClaim((result) => result.Issuer == issuer))
                    {
                        request.Headers.Add("AuthorizationStatus", Convert.ToInt32(HttpStatusCode.Accepted).ToString());
                    }
                    else
                    {
                        request.Headers.Add("AuthorizationStatus", Convert.ToInt32(HttpStatusCode.Unauthorized).ToString());

                    }
                }
                else
                {
                    request.Headers.Add("AuthorizationStatus", Convert.ToInt32(HttpStatusCode.Unauthorized).ToString());
                }
            }
            catch (SecurityTokenExpiredException)
            {
                request.Headers.Add("AuthorizationStatus", Convert.ToInt32(HttpStatusCode.Gone).ToString());
            }
            catch (Exception ex)
            {
                request.Headers.Add("AuthorizationStatus", Convert.ToInt32(HttpStatusCode.ExpectationFailed).ToString());
            }
        }
    }
}
