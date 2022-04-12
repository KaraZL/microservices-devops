using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using System;
using Microsoft.EntityFrameworkCore;
using Courses.API.Data;
using Courses.API.MessageBus;
using Courses.API.Grpc;
using Courses.API.Policies;
using HealthChecks.UI.Client;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using OpenTelemetry.Trace;
using OpenTelemetry.Resources;

namespace Courses.API
{
    public class Startup
    {
        private readonly IWebHostEnvironment _env;

        public Startup(IConfiguration configuration, IWebHostEnvironment env)
        {
            Configuration = configuration;
            _env = env;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<ClientPolicy>();
            services.AddScoped<ICoursesRepository, SqlCoursesRepository>();
            services.AddScoped<ICourseDataClient, CourseDataClient>();

            //Connections are meant to be long-lived.
            //Opening a connection for every operation (e.g. publishing a message) would be very inefficient and is highly discouraged.
            services.AddSingleton<IMessageBusClient, MessageBusClient>();

            services.AddDbContext<DatabaseContext>(options => {
                options.UseSqlServer(Configuration.GetConnectionString("SqlDatabase"));
            });

            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Courses.API", Version = "v1" });
            });

            //HealthChecks
            services
                .AddHealthChecks()
                .AddDbContextCheck<DatabaseContext>()
                //amqp://localhost:5672
                .AddRabbitMQ(
                    rabbitConnectionString: $"amqp://{Configuration["RabbitMQHost"]}:{Configuration["RabbitMQPort"]}",
                    name: "Courses.API : RabbitMQ Publisher",
                    failureStatus: HealthStatus.Degraded);

            //Telemetry
            services.AddOpenTelemetryTracing(builder =>
            {
                builder
                    .SetResourceBuilder(resourceBuilder: ResourceBuilder.CreateDefault().AddService("CoursesApi")) //Name of the service
                    .AddAspNetCoreInstrumentation(options => { })
                    .AddHttpClientInstrumentation(options => { })
                    .AddGrpcClientInstrumentation(options =>
                    {
                        options.SuppressDownstreamInstrumentation = true; //prevents the HttpClient instrumentation from generating an additional activity
                    })
                    .AddEntityFrameworkCoreInstrumentation(options => { options.SetDbStatementForText = true; })
                    .AddZipkinExporter(options =>
                    {
                        options.Endpoint = new Uri(Configuration["ZipkinUri"]); //default http://localhost:9411/api/v2/spans
                    });

            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Courses.API v1"));
            }
            else
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Courses.API v1"));
            }

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapHealthChecks("/hc", new Microsoft.AspNetCore.Diagnostics.HealthChecks.HealthCheckOptions()
                {
                    Predicate = _ => true,
                    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
                });
                endpoints.MapControllers();
            });

            PrepDb.PrepPopulation(app);
        }
    }
}
