using Microsoft.AspNetCore.Builder;

namespace CustomerOrder.Infrastructure.Extensions;

public static partial class ApplicationBuilderExtensions
{
    public static IApplicationBuilder AddInfrastructureApplicationConfigurations(this IApplicationBuilder app)
    {
        RunDbMigration(app);

        return app;
    }
}
