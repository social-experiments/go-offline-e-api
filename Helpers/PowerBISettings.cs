using System;

namespace goOfflineE.Helpers
{
    public class PowerBISettings
    {
        public static string ApplicationId => Environment.GetEnvironmentVariable("ApplicationId");
        public static string ApplicationSecret => Environment.GetEnvironmentVariable("ApplicationSecret");
        public static string AuthorityUrl => Environment.GetEnvironmentVariable("AuthorityUrl");
        public static string ResourceUrl => Environment.GetEnvironmentVariable("ResourceUrl");
        public static string ApiUrl => Environment.GetEnvironmentVariable("ApiUrl");
        public static string EmbedUrlBase => Environment.GetEnvironmentVariable("EmbedUrlBase");
        public static string UserName => Environment.GetEnvironmentVariable("PUserName");
        public static string Password => Environment.GetEnvironmentVariable("Password");
        public static string WorkspaceId => Environment.GetEnvironmentVariable("PWorkspaceId");
        public static string ReportId => Environment.GetEnvironmentVariable("ReportId");
        public static string DashboardId => Environment.GetEnvironmentVariable("DashboardId");
        public static string TokenType => Environment.GetEnvironmentVariable("TokenType");


    }
}
