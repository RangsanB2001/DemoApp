using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;

namespace Middlewares
{
    public class ForwardedPrefixMiddleware
    {
        private readonly RequestDelegate _next;

        public ForwardedPrefixMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            if (context.Request.Headers.TryGetValue("X-Forwarded-Prefix", out var pathPrefixValues) && pathPrefixValues.Count > 0)
            {
                var pathPrefix = pathPrefixValues.Last();
                context.Request.PathBase = new PathString(pathPrefix);
                if (context.Request.Path.StartsWithSegments(context.Request.PathBase, out var path))
                {
                    context.Request.Path = path;
                }
            }

            await _next(context);
        }
    }

    public static class ForwardedPrefixMiddlewareExtensions
    {
        public static IApplicationBuilder UseForwardedPrefixMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<ForwardedPrefixMiddleware>();
        }
    }
}
