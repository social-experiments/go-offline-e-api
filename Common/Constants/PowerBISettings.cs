namespace goOfflineE.Common.Constants
{
    using System;

    /// <summary>
    /// Defines the <see cref="PowerBISettings" />.
    /// </summary>
    public class PowerBISettings
    {
        /// <summary>
        /// Gets the ApplicationId.
        /// </summary>
        public static string ApplicationId => Environment.GetEnvironmentVariable("ApplicationId");

        /// <summary>
        /// Gets the ApplicationSecret.
        /// </summary>
        public static string ApplicationSecret => Environment.GetEnvironmentVariable("ApplicationSecret");

        /// <summary>
        /// Gets the AuthorityUrl.
        /// </summary>
        public static string AuthorityUrl => Environment.GetEnvironmentVariable("AuthorityUrl");

        /// <summary>
        /// Gets the ResourceUrl.
        /// </summary>
        public static string ResourceUrl => Environment.GetEnvironmentVariable("ResourceUrl");

        /// <summary>
        /// Gets the ApiUrl.
        /// </summary>
        public static string ApiUrl => Environment.GetEnvironmentVariable("ApiUrl");

        /// <summary>
        /// Gets the EmbedUrlBase.
        /// </summary>
        public static string EmbedUrlBase => Environment.GetEnvironmentVariable("EmbedUrlBase");

        /// <summary>
        /// Gets the UserName.
        /// </summary>
        public static string UserName => Environment.GetEnvironmentVariable("PUserName");

        /// <summary>
        /// Gets the Password.
        /// </summary>
        public static string Password => Environment.GetEnvironmentVariable("Password");

        /// <summary>
        /// Gets the WorkspaceId.
        /// </summary>
        public static string WorkspaceId => Environment.GetEnvironmentVariable("PWorkspaceId");

        /// <summary>
        /// Gets the ReportId.
        /// </summary>
        public static string ReportId => Environment.GetEnvironmentVariable("ReportId");

        /// <summary>
        /// Gets the DashboardId.
        /// </summary>
        public static string DashboardId => Environment.GetEnvironmentVariable("DashboardId");

        /// <summary>
        /// Gets the TokenType.
        /// </summary>
        public static string TokenType => Environment.GetEnvironmentVariable("TokenType");
    }
}
