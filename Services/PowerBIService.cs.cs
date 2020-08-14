using goOfflineE.Azure.Function.Api.Models;
using goOfflineE.Helpers;
using Microsoft.PowerBI.Api;
using Microsoft.PowerBI.Api.Models;
using Microsoft.Rest;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace goOfflineE.Services
{
    public class PowerBIService : IPowerBIService
    {
        private async Task<string> PowerBIAccessToken()
        {
            using (var client = new HttpClient())
            {
                var form = new Dictionary<string, string>();
                form["grant_type"] = "password";
                form["resource"] = PowerBISettings.ResourceUrl;
                form["username"] = PowerBISettings.UserName;
                form["password"] = PowerBISettings.Password;
                form["client_id"] = PowerBISettings.ApplicationId;
                form["client_secret"] = PowerBISettings.ApplicationSecret;
                form["scope"] = "openid";
                client.DefaultRequestHeaders.TryAddWithoutValidation("Content-Type", "application/x-www-form-urlencoded");
                using (var formContent = new FormUrlEncodedContent(form))
                using (var response =
                    await client.PostAsync(PowerBISettings.AuthorityUrl, formContent))
                {
                    var body = await response.Content.ReadAsStringAsync();
                    var jsonBody = JObject.Parse(body);
                    var errorToken = jsonBody.SelectToken("error");
                    if (errorToken != null)
                    {
                        throw new Exception(errorToken.Value<string>());
                    }
                    return jsonBody.SelectToken("access_token").Value<string>();
                }
            }
        }

        public async Task<IEnumerable<PowerBIResponse>> GetPowerBIAccessToken()
        {
            var accessToken = await PowerBIAccessToken();
            var tokenCredentials = new TokenCredentials(accessToken, "Bearer");
            var powerBIResponses = new List<PowerBIResponse>();
            using (var client = new PowerBIClient(new Uri(PowerBISettings.ApiUrl), tokenCredentials))
            {

                var workspaceId = Guid.Parse(PowerBISettings.WorkspaceId);
                var reportId = Guid.Parse(PowerBISettings.ReportId);
                var dashboardId = Guid.Parse(PowerBISettings.DashboardId);

                var report = await client.Reports.GetReportInGroupAsync(workspaceId, reportId);
                var generateTokenRequestParameters = new GenerateTokenRequest(accessLevel: "view");
                var reportTokenResponse = await client.Reports.GenerateTokenAsync(workspaceId, reportId, generateTokenRequestParameters);

                powerBIResponses.Add(new PowerBIResponse
                {
                    ReportId = reportId.ToString(),
                    ReportType = "report",
                    TokenType = PowerBISettings.TokenType,
                    AccessToken = reportTokenResponse.Token,
                    EmbedUrl = report.EmbedUrl
                });

                var dashboard = await client.Dashboards.GetDashboardAsync(workspaceId, dashboardId);
                var dashboardTokenResponse = await client.Dashboards.GenerateTokenAsync(workspaceId, dashboardId, generateTokenRequestParameters);
                powerBIResponses.Add(new PowerBIResponse
                {
                    ReportId = dashboardId.ToString(),
                    ReportType = "dashboard",
                    TokenType = PowerBISettings.TokenType,
                    AccessToken = dashboardTokenResponse.Token,
                    EmbedUrl = dashboard.EmbedUrl
                });

                return powerBIResponses;
            }
        }
    }
}
