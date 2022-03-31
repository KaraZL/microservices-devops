using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using GeneralStore.API.Data;
using Microsoft.EntityFrameworkCore;
using GeneralStore.API.EventProcessing;
using GeneralStore.API.MessageBus;
using GeneralStore.API.Grpc;
using Microsoft.AspNetCore.Http;
using System.IO;
using GeneralStore.API.Policies;

namespace GeneralStore.API
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
            services.AddDbContext<DatabaseContext>(options => {
                options.UseSqlServer(Configuration.GetConnectionString("SqlDatabase"));
            });

            services.AddSingleton<ClientPolicy>();

            services.AddGrpc();

            services.AddSingleton<IEventProcessor, EventProcessor>();
            //BackgroundService est une classe de base pour l’implémentation d’une exécution longue IHostedService .
            services.AddHostedService<MessageBusSubscriber>();
            services.AddScoped<IStoreRepository, SqlStoreRepository>();

            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
            services.AddRazorPages();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
            }

            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapRazorPages();

                //Pour chaque service
                endpoints.MapGrpcService<GrpcCourseService>();

                //envoie les informations du contrat entre serveur et client au client
                endpoints.MapGet("/protos/courses.proto", async context =>
                {
                    await context.Response.WriteAsync(File.ReadAllText("Protos/courses.proto"));
                });
            });

            PrepDb.PrepPopulation(app);
        }
    }
}
