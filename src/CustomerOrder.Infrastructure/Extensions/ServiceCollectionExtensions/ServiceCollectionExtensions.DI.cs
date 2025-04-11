using CustomerOrder.Application.Interfaces;
using CustomerOrder.Common;
using CustomerOrder.Domain.Services;
using CustomerOrder.Infrastructure.DomainServices;
using CustomerOrder.Infrastructure.Repositorie;
using CustomerOrder.Infrastructure.Utils;
using Microsoft.Extensions.DependencyInjection;

namespace CustomerOrder.Infrastructure.Extensions.ServiceCollectionExtensions;
public static partial class ServiceCollectionExtensions
{
    private static IServiceCollection AddDI(this IServiceCollection services)
    {
        services.AddScoped<ICalculateOrderTotalPriceService, CalculateOrderTotalPriceService>();
        services.AddScoped(typeof(IGenericRepository<,>), typeof(GenericRepository<,>));
        services.AddScoped<IUnitOfWork, UnitOfWork>();

        return services;
    }
}
