using Common.Logging;
using HealthChecks.UI.Client;
using MediatR;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Series.API.Data;
using Series.Application;
using Series.Infrastructure;
using Serilog;
using StackExchange.Redis;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);
builder.Host.UseSerilog(SeriLogger.Configure);

//From Clean architectures services
builder.Services.AddApplicationServices();
builder.Services.AddInfrastructureServices(builder.Configuration);

// Add services to the container.
builder.Services.AddMediatR(typeof(MediatRAssembly).Assembly);

builder.Services.AddHealthChecks()
                .AddRedis(builder.Configuration.GetConnectionString("Redis"), name: "Series.API : Redis", HealthStatus.Degraded)
                .AddElasticsearch(builder.Configuration.GetConnectionString("ElasticUri"), name: "Series.API : Elastic Search", HealthStatus.Degraded);

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();
app.MapHealthChecks("/hc", new Microsoft.AspNetCore.Diagnostics.HealthChecks.HealthCheckOptions()
{
    Predicate = _ => true,
    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
});

app.Run();
