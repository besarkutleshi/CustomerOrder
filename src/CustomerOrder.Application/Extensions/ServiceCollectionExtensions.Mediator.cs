using MediatR;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace CustomerOrder.Application.Extensions;

public static partial class ServiceCollectionExtensions
{
    private static IServiceCollection AddMediator(this IServiceCollection services)
    {
        services.AddMediatR(Assembly.GetExecutingAssembly());

        return services;
    }
}