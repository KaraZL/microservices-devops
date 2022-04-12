using Common.Logging;
using HealthChecks.UI.Client;
using MediatR;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Series.API.Data;
using Series.Application;
using Series.Infrastructure;
using Serilog;
using OpenTelemetry.Trace;
using OpenTelemetry.Resources;
using StackExchange.Redis;

var builder = WebApplication.CreateBuilder(args);
builder.Host.UseSerilog(SeriLogger.Configure);

//From Clean architectures services
builder.Services.AddApplicationServices();

//Telemetry a besoin d'une instance de ConnectionMultiplexer pour Redis, alors on l'envoie depuis ici pour récuperer l'instance
var redisConnection = ConnectionMultiplexer.Connect(builder.Configuration.GetConnectionString("Redis"));
builder.Services.AddInfrastructureServices(builder.Configuration, redisConnection);

// Add services to the container.
builder.Services.AddMediatR(typeof(MediatRAssembly).Assembly);

builder.Services.AddHealthChecks()
                .AddRedis(builder.Configuration.GetConnectionString("Redis"), name: "Series.API : Redis", HealthStatus.Degraded)
                .AddElasticsearch(builder.Configuration.GetConnectionString("ElasticUri"), name: "Series.API : Elastic Search", HealthStatus.Degraded);

builder.Services.AddOpenTelemetryTracing(telemetryBuilder =>
{
    telemetryBuilder
        .SetResourceBuilder(resourceBuilder: ResourceBuilder.CreateDefault().AddService("SeriesApi"))
        .AddAspNetCoreInstrumentation()
        .AddHttpClientInstrumentation()
        .AddRedisInstrumentation(redisConnection, options => { options.SetVerboseDatabaseStatements = true; })
        .AddZipkinExporter(options =>
        {
            options.Endpoint = new Uri(builder.Configuration["ZipkinUri"]);
        });
});

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
