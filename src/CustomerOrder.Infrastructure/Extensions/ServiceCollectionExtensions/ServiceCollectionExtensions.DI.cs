using CustomerOrder.Domain.Services;
using CustomerOrder.Infrastructure.DomainServices;
using Microsoft.Extensions.DependencyInjection;

namespace CustomerOrder.Infrastructure.Extensions.ServiceCollectionExtensions;
public static partial class ServiceCollectionExtensions
{
    private static IServiceCollection AddDI(this IServiceCollection services)
    {
        services.AddScoped<ICalculateOrderTotalPriceService, CalculateOrderTotalPriceService>();

        return services;
    }
}
