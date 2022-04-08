using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using Movies.DataAccess.Repository;
using Movies.DataAccess.Data;
using Movies.DataAccess;
using Movies.API.Migrations;
using FluentMigrator.Runner;
using System.Reflection;
using MediatR;
using HealthChecks.UI.Client;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace Movies.API
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {

            //From DataAccess - ApplicationServiceRegistration
            services.AddApplicationServices();

            //Necessite IConfiguration dans Class Library
            //Necessaire dans DatabaseSeed
            services.AddSingleton<DapperContext>();

            //Permet de récuperer ce service depuis PrepDb dans Configure()
            services.AddSingleton<DatabaseSeed>();

            //it is injected in mediatR handler classes
            services.AddSingleton<IMovieRepo, MovieRepo>();

            //Inject in MovieRepo
            services.AddSingleton<SqlDataAccess>();

            services.AddMediatR(typeof(MediatRAssembly).Assembly);
            services.AddAutoMapper(typeof(MediatRAssembly).Assembly);

            //IMigrationRunner
            services.AddLogging(c => c.AddFluentMigratorConsole())
                .AddFluentMigratorCore()
                .ConfigureRunner(c => c.AddSqlServer2016()
                    .WithGlobalConnectionString(Configuration.GetConnectionString("SqlDatabase"))
                    .ScanIn(Assembly.GetExecutingAssembly()).For.Migrations());

            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Movies.API", Version = "v1" });
            });

            services.AddHealthChecks()
                    .AddSqlServer(connectionString: Configuration.GetConnectionString("SqlDatabase"), name: "Movies.API : Sql Server", failureStatus: HealthStatus.Degraded);

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Movies.API v1"));
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

            PrepDb.Migrate(app);
            
        }
    }
}
