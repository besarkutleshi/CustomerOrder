using CustomerOrder.API.Middlewares;

namespace CustomerOrder.API.Extensions;

public static partial class ServiceCollectionExtensions
{
    private static IServiceCollection AddMiddlewares(this IServiceCollection services)
    {
        services.AddTransient<ServiceMiddleware>();

        return services;
    }
}
