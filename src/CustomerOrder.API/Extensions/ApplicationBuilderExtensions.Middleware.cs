using CustomerOrder.API.Middlewares;

namespace CustomerOrder.API.Extensions;

public static partial class ApplicationBuilderExtensions
{
    private static IApplicationBuilder UseServiceMiddleware(this IApplicationBuilder app)
    {
        app.UseMiddleware<ServiceMiddleware>();
        app.UseMiddleware<RequestLogContextMiddleware>();

        return app;
    }
}
