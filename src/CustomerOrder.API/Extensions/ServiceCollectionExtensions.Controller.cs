using System.Text.Json.Serialization;
using System.Text.Json;

namespace CustomerOrder.API.Extensions;

public static partial class ServiceCollectionExtensions
{
    private static IServiceCollection AddController(this IServiceCollection services)
    {
        services.AddControllers().AddJsonOptions(opt =>
        {
            opt.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
            opt.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
            opt.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
            opt.JsonSerializerOptions.WriteIndented = true;
            opt.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
        });

        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen();

        return services;
    }
}