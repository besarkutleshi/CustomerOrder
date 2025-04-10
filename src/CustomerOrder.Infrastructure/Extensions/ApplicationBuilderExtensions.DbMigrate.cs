using CustomerOrder.Infrastructure.Persistence;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Serilog;

namespace CustomerOrder.Infrastructure.Extensions;
public static partial class ApplicationBuilderExtensions
{
    private static ILogger _logger = null!;
    private static IServiceScopeFactory _serviceScopeFactory = null!;

    private static IApplicationBuilder RunDbMigration(this IApplicationBuilder app)
    {
        try
        {
            _serviceScopeFactory = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>();
            _logger = app.ApplicationServices.GetRequiredService<ILogger>();

            Migrate(_serviceScopeFactory);

            return app;
        }
        catch (Exception)
        {
            _logger.Error("Application could not connect to database");

            return app;
        }
    }

    private static void Migrate(IServiceScopeFactory serviceScopeFactory)
    {
        try
        {
            var serviceScope = serviceScopeFactory.CreateScope();
            using var context = serviceScope.ServiceProvider.GetService<AppDbContext>();

            context?.Database.Migrate();
        }
        catch (Exception ex)
        {
            _logger.Error("ERROR: Message: {ex.Message}, Stack: {ex.StackTrace}", ex.Message, ex.StackTrace);

            throw;
        }
    }
}