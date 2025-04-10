using CustomerOrder.Application.Behaviours;
using CustomerOrder.Common.Helpers;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace CustomerOrder.Application.Extensions;
public static partial class ServiceCollectionExtensions
{
    public static IServiceCollection AddDI(this IServiceCollection services)
    {
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehaviour<,>));
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(LoggingBehavior<,>));
        services.AddScoped<IRequestService, RequestService>();

        return services;
    }
}
