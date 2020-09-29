namespace goOfflineE.Models
{
    using goOfflineE.Common.Enums;
    using System;
    using System.Security.Claims;

    /// <summary>
    /// Contains the result of an access token check.
    /// </summary>
    public sealed class AccessTokenResult
    {
        /// <summary>
        /// Prevents a default instance of the <see cref="AccessTokenResult"/> class from being created.
        /// </summary>
        private AccessTokenResult()
        {
        }

        /// <summary>
        /// Gets the security principal associated with a valid token..
        /// </summary>
        public ClaimsPrincipal Principal { get; private set; }

        /// <summary>
        /// Gets the status of the token, i.e. whether it is valid..
        /// </summary>
        public AccessTokenStatus Status { get; private set; }

        /// <summary>
        /// Gets the Exception
        /// Gets any exception encountered when validating a token..
        /// </summary>
        public Exception Exception { get; private set; }

        /// <summary>
        /// Returns a valid token.
        /// </summary>
        /// <param name="principal">The principal<see cref="ClaimsPrincipal"/>.</param>
        /// <returns>The <see cref="AccessTokenResult"/>.</returns>
        public static AccessTokenResult Success(ClaimsPrincipal principal)
        {
            return new AccessTokenResult
            {
                Principal = principal,
                Status = AccessTokenStatus.Valid
            };
        }

        /// <summary>
        /// Returns a result that indicates the submitted token has expired.
        /// </summary>
        /// <returns>The <see cref="AccessTokenResult"/>.</returns>
        public static AccessTokenResult Expired()
        {
            return new AccessTokenResult
            {
                Status = AccessTokenStatus.Expired
            };
        }

        /// <summary>
        /// Returns a result to indicate that there was an error when processing the token.
        /// </summary>
        /// <param name="ex">The ex<see cref="Exception"/>.</param>
        /// <returns>The <see cref="AccessTokenResult"/>.</returns>
        public static AccessTokenResult Error(Exception ex)
        {
            return new AccessTokenResult
            {
                Status = AccessTokenStatus.Error,
                Exception = ex
            };
        }

        /// <summary>
        /// Returns a result in response to no token being in the request.
        /// </summary>
        /// <returns>.</returns>
        public static AccessTokenResult NoToken()
        {
            return new AccessTokenResult
            {
                Status = AccessTokenStatus.NoToken
            };
        }
    }
}
