using System;

namespace Educati.Azure.Function.Api.Helpers
{
    public static class FunctionConfigs
    {

        public static string IssuerToken => Environment.GetEnvironmentVariable("ISSUER_TOKEN");
        public static string Audience => Environment.GetEnvironmentVariable("AUDIENCE");
        public static string Issuer => Environment.GetEnvironmentVariable("ISSUER");

        //Table storage connection string
        public static string TableStorageConnstionString => Environment.GetEnvironmentVariable("AzureWebJobsStorage");

    }
}
