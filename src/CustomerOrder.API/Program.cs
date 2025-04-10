using CustomerOrder.Infrastructure.Extensions.ServiceCollectionExtensions;
using CustomerOrder.Infrastructure.Extensions;
using CustomerOrder.Application.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddInfrastructureServices(builder.Configuration);
builder.Services.AddApplicationServiceCollectionConfigurations(builder.Configuration);

var app = builder.Build();

app.AddInfrastructureApplicationConfigurations();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.Run();