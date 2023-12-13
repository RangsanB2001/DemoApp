using Microsoft.Extensions.DependencyInjection;

namespace Helpers.CallHttpClientHelper
{
    public static class CallHttpClientHelperExtensions
    {
        public static IServiceCollection AddICallHttpClientHelper(this IServiceCollection services)
        {
            services.AddScoped<ICallHttpClientHelper, CallHttpClientHelper>();
            return services;
        }
    }
}
