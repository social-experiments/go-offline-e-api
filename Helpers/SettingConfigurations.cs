using System;
using System.Security.Cryptography;

namespace goOfflineE.Helpers
{
    public static class SettingConfigurations
    {

        public static string IssuerToken => Environment.GetEnvironmentVariable("ISSUER_TOKEN");
        public static string Audience => Environment.GetEnvironmentVariable("AUDIENCE");
        public static string Issuer => Environment.GetEnvironmentVariable("ISSUER");

        //Email configurations
        public static string SMTPServer => Environment.GetEnvironmentVariable("SMTP_SERVER");
        public static Int16 SMTPPort => Convert.ToInt16(Environment.GetEnvironmentVariable("SMTP_PORT"));
        public static string SMTPUser => Environment.GetEnvironmentVariable("SMTP_USER");
        public static string SMTPPassword => Environment.GetEnvironmentVariable("SMTP_PASSWORD");

        //Azure storage
        public static string AzureWebJobsStorage => Environment.GetEnvironmentVariable("AzureWebJobsStorage");
        public static string AccountKey => Environment.GetEnvironmentVariable("AccountKey");
        

        public static string GetRandomPassword(int length)
        {
            byte[] rgb = new byte[length];
            RNGCryptoServiceProvider rngCrypt = new RNGCryptoServiceProvider();
            rngCrypt.GetBytes(rgb);
            return Convert.ToBase64String(rgb);
        }
    }
}
