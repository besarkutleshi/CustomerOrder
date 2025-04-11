namespace CustomerOrder.API.Extensions;

public static partial class ServiceCollectionExtensions
{
    public static IServiceCollection AddPresentationServices(this IServiceCollection services)
    {
        AddController(services);
        AddMiddlewares(services);
        services.AddHealthChecks();

        return services;
    }
}