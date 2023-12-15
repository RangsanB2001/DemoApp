using Microsoft.AspNetCore.Http;
using System.Net.Http.Headers;
using System.Text;

namespace Helpers.CallHttpClientHelper
{
    public class CallHttpClientHelper : ICallHttpClientHelper
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CallHttpClientHelper(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<(bool success, string body)> SendJson(string uri, HttpMethod method, string content, bool sendBearerToken = false, string? bearerToken = null)
        {
            using var httpClient = new HttpClient();

            var clientIp = _httpContextAccessor?.HttpContext?.Request.Headers["X-Forwarded-For"].FirstOrDefault();
            if (string.IsNullOrEmpty(clientIp))
            {
                clientIp = _httpContextAccessor?.HttpContext?.Connection?.RemoteIpAddress?.ToString();
            }

            httpClient.DefaultRequestHeaders.Add("X-Forwarded-For", clientIp);

            using var httpRequest = new HttpRequestMessage
            {
                RequestUri = new Uri(uri),
                Method = method
            };

            if (sendBearerToken)
            {
                if (!string.IsNullOrEmpty(bearerToken))
                {
                    httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", bearerToken);
                }
            }


            httpRequest.Headers.Add("Accept", "application/json");

            if (!string.IsNullOrWhiteSpace(content))
            {
                httpRequest.Content = new StringContent(content, Encoding.UTF8, "application/json");
            }

            using var httpResponse = await httpClient.SendAsync(httpRequest);
            var body = await httpResponse.Content.ReadAsStringAsync();
            var success = httpResponse.IsSuccessStatusCode;

            return (success, body);
        }
    }
}
