namespace Helpers.CallHttpClientHelper
{
    public interface ICallHttpClientHelper
    {
        Task<(bool success, string body)> SendJson(
            string uri, HttpMethod method,
            string content, bool sendBearerToken = false,
            string? bearerToken = null
            );
    }
}
