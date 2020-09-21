namespace goOfflineE.Common.Constants
{
    using System;
    using System.Security.Cryptography;

    /// <summary>
    /// Defines the <see cref="SettingConfigurations" />.
    /// </summary>
    public static class SettingConfigurations
    {
        /// <summary>
        /// Gets the IssuerToken.
        /// </summary>
        public static string IssuerToken => Environment.GetEnvironmentVariable("ISSUER_TOKEN");

        /// <summary>
        /// Gets the Audience.
        /// </summary>
        public static string Audience => Environment.GetEnvironmentVariable("AUDIENCE");

        /// <summary>
        /// Gets the Issuer.
        /// </summary>
        public static string Issuer => Environment.GetEnvironmentVariable("ISSUER");

        /// <summary>
        /// Gets the SMTPServer.
        /// </summary>
        public static string SMTPServer => Environment.GetEnvironmentVariable("SMTP_SERVER");

        /// <summary>
        /// Gets the SMTPPort.
        /// </summary>
        public static Int16 SMTPPort => Convert.ToInt16(Environment.GetEnvironmentVariable("SMTP_PORT"));

        /// <summary>
        /// Gets the SMTPUser.
        /// </summary>
        public static string SMTPUser => Environment.GetEnvironmentVariable("SMTP_USER");

        /// <summary>
        /// Gets the SMTPPassword.
        /// </summary>
        public static string SMTPPassword => Environment.GetEnvironmentVariable("SMTP_PASSWORD");

        /// <summary>
        /// Gets the AzureWebJobsStorage.
        /// </summary>
        public static string AzureWebJobsStorage => Environment.GetEnvironmentVariable("AzureWebJobsStorage");

        /// <summary>
        /// Gets the AccountKey.
        /// </summary>
        public static string AccountKey => Environment.GetEnvironmentVariable("AccountKey");

        /// <summary>
        /// Gets the CognitiveServiceKey.
        /// </summary>
        public static string CognitiveServiceKey => Environment.GetEnvironmentVariable("CognitiveServiceKey");

        /// <summary>
        /// Gets the CognitiveServiceEndPoint.
        /// </summary>
        public static string CognitiveServiceEndPoint => Environment.GetEnvironmentVariable("CognitiveServiceEndPoint");

        /// <summary>
        /// Gets the WebSiteUrl.
        /// </summary>
        public static string WebSiteUrl => Environment.GetEnvironmentVariable("WEB_SITE");

        /// <summary>
        /// The GetRandomPassword.
        /// </summary>
        /// <param name="length">The length<see cref="int"/>.</param>
        /// <returns>The <see cref="string"/>.</returns>
        public static string GetRandomPassword(int length)
        {
            byte[] rgb = new byte[length];
            RNGCryptoServiceProvider rngCrypt = new RNGCryptoServiceProvider();
            rngCrypt.GetBytes(rgb);
            return Convert.ToBase64String(rgb);
        }
    }
}
