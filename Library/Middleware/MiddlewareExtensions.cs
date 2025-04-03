using Microsoft.AspNetCore.Builder;

namespace Library.Common.Middleware
{
    public static class MiddlewareExtensions
    {
        public static IApplicationBuilder UseExceptionMiddleware(this IApplicationBuilder app) => 
            app.UseMiddleware<ExceptionMiddleware>();
    }
}
