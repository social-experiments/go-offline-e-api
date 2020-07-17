using Educati.Helpers.Enums;
using Educati.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Educati.Helpers.Attributes
{

    /// <summary>
    /// Base class for authenticated service which checks the incoming JWT token.
    /// </summary>
    public abstract class AuthenticationFilter : IFunctionInvocationFilter
    {
        private const string AUTH_HEADER_NAME = "Authorization";
        private const string BEARER_PREFIX = "Bearer ";
        protected AccessTokenStatus accessTokenStatus;

        /// <summary>
        ///     Pre-execution filter.
        /// </summary>
        /// <remarks>
        ///     This mechanism can be used to extract the authentication information. 
        /// </remarks>
        public Task  OnExecutingAsync(FunctionExecutingContext executingContext, CancellationToken cancellationToken)
        {
            var httpRequest = executingContext.Arguments.First().Value as HttpRequest;
            accessTokenStatus = ValidateToken(httpRequest).Status;
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

        private AccessTokenResult ValidateToken(HttpRequest request)
        {
            try
            {
                var audience = FunctionConfigs.Audience;
                var issuer = FunctionConfigs.Issuer;
                var mySecurityKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(FunctionConfigs.IssuerToken));
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
                    return AccessTokenResult.Success(result);
                }
                else
                {
                    return AccessTokenResult.NoToken();
                }
            }
            catch (SecurityTokenExpiredException)
            {
                return AccessTokenResult.Expired();
            }
            catch (Exception ex)
            {
                return AccessTokenResult.Error(ex);
            }
        }
    }
}
