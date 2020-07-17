namespace Educati.Azure.Function.Api.Helpers
{
    public static class FunctionConfigs
    {

        //TO DO: remove hardcoded values with set Environment variables
        public static string IssuerToken => "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJuYW1laWQiOiIxIiwibmJmIjoxNTk0OTg3MTY3LCJleHAiOjE1OTU1OTE5NjcsImlhdCI6MTU5NDk4NzE2NywiaXNzIjoiSXNzdWVyIiwiYXVkIjoiQXVkaWVuY2UifQ.fEPJyK354PXszYdNncqBXl7RCgUN7v5fRs2G4zyaH2Q"; // Environment.GetEnvironmentVariable("IssuerToken");
        public static string Audience => "Audience"; // Environment.GetEnvironmentVariable("Audience");
        public static string Issuer => "Issuer"; // Environment.GetEnvironmentVariable("Issuer");
    }
}
