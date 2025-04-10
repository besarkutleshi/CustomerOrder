using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CustomerOrder.Infrastructure.Extensions.ServiceCollectionExtensions;
public static partial class ServiceCollectionExtensions
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddEF(configuration);
        services.AddDI();

        return services;
    }
}