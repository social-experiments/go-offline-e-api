namespace goOfflineE.Helpers.Attributes
{
    using goOfflineE.Common.Constants;
    using Microsoft.AspNetCore.Http;
    using Microsoft.Azure.WebJobs.Host;
    using Microsoft.Extensions.Primitives;
    using Microsoft.IdentityModel.Tokens;
    using System;
    using System.IdentityModel.Tokens.Jwt;
    using System.Linq;
    using System.Net;
    using System.Security.Claims;
    using System.Text;
    using System.Threading;
    using System.Threading.Tasks;

    /// <summary>
    /// Base class for authenticated service which checks the incoming JWT token.
    /// </summary>
    public abstract class AuthenticationFilter : IFunctionInvocationFilter
    {
        /// <summary>
        /// Defines the AUTH_HEADER_NAME.
        /// </summary>
        private const string AUTH_HEADER_NAME = "Authorization";

        /// <summary>
        /// Defines the BEARER_PREFIX.
        /// </summary>
        private const string BEARER_PREFIX = "Bearer ";

        /// <summary>
        /// Pre-execution filter.
        /// </summary>
        /// <param name="executingContext">The executingContext<see cref="FunctionExecutingContext"/>.</param>
        /// <param name="cancellationToken">The cancellationToken<see cref="CancellationToken"/>.</param>
        /// <returns>The <see cref="Task"/>.</returns>
        public Task OnExecutingAsync(FunctionExecutingContext executingContext, CancellationToken cancellationToken)
        {
            var httpRequest = executingContext.Arguments.First().Value as HttpRequest;
            ValidateToken(httpRequest);
            return Task.CompletedTask;
        }

        /// <summary>
        /// Post-execution filter.
        /// </summary>
        /// <param name="executedContext">The executedContext<see cref="FunctionExecutedContext"/>.</param>
        /// <param name="cancellationToken">The cancellationToken<see cref="CancellationToken"/>.</param>
        /// <returns>The <see cref="Task"/>.</returns>
        public Task OnExecutedAsync(FunctionExecutedContext executedContext, CancellationToken cancellationToken)
        {
            // Nothing.
            return Task.CompletedTask;
        }

        /// <summary>
        /// The AuthorizationStatus.
        /// </summary>
        /// <param name="request">The request<see cref="HttpRequest"/>.</param>
        /// <returns>The <see cref="HttpStatusCode"/>.</returns>
        public HttpStatusCode AuthorizationStatus(HttpRequest request)
        {
            StringValues keys;
            if (!request.Headers.TryGetValue("AuthorizationStatus", out keys))
            {
                return HttpStatusCode.ExpectationFailed;
            }

            Int16 httpStatus = Convert.ToInt16(keys.First());
            return (HttpStatusCode)httpStatus;
        }

        /// <summary>
        /// The ValidateToken.
        /// </summary>
        /// <param name="request">The request<see cref="HttpRequest"/>.</param>
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
                        var tenantId = result.Claims.FirstOrDefault(c => c.Type == ClaimTypes.GroupSid)?.Value;
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
            catch (Exception)
            {
                request.Headers.Add("AuthorizationStatus", Convert.ToInt32(HttpStatusCode.ExpectationFailed).ToString());
            }
        }
    }
}
