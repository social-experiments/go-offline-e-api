using System;

namespace Educati.Azure.Function.Api.Helpers
{
    public static class SettingConfigurations
    {

        public static string IssuerToken => Environment.GetEnvironmentVariable("ISSUER_TOKEN");
        public static string Audience => Environment.GetEnvironmentVariable("AUDIENCE");
        public static string Issuer => Environment.GetEnvironmentVariable("ISSUER");

        //Email configurations
        public static string SMTPServer => Environment.GetEnvironmentVariable("SMTP_SERVER");
        public static Int16 SMTPPort => Convert.ToInt16( Environment.GetEnvironmentVariable("SMTP_PORT"));
        public static string SMTPUser => Environment.GetEnvironmentVariable("SMTP_USER");
        public static string SMTPPassword => Environment.GetEnvironmentVariable("SMTP_PASSWORD");

        //Table storage connection string
        public static string TableStorageConnstionString => Environment.GetEnvironmentVariable("AzureWebJobsStorage");


    }
}
