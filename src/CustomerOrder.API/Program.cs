using CustomerOrder.Infrastructure.Extensions.ServiceCollectionExtensions;
using CustomerOrder.Infrastructure.Extensions;
using CustomerOrder.Application.Extensions;
using Serilog;
using CustomerOrder.API.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddInfrastructureServices(builder.Configuration);
builder.Services.AddApplicationServiceCollectionConfigurations(builder.Configuration);
builder.Host.UseSerilog((context, configuration) =>
{
    configuration.ReadFrom.Configuration(context.Configuration);
});
builder.Services.AddPresentationServices();

var app = builder.Build();

app.AddInfrastructureApplicationConfigurations();
app.AddPersistentApplicationBuilderConfigurations();

app.UseHttpsRedirection();

app.Run();