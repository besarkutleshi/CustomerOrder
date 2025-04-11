using Serilog;

namespace CustomerOrder.API.Extensions;

public static partial class ApplicationBuilderExtensions
{
    public static IApplicationBuilder AddPersistentApplicationBuilderConfigurations(this WebApplication app)
    {
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        UseServiceMiddleware(app);

        app.UseSerilogRequestLogging();
        app.MapHealthChecks("/health");

        return app;
    }
}