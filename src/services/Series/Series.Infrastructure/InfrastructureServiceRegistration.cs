using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Series.API.Data;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Series.Infrastructure
{
    public static class InfrastructureServiceRegistration
    {
        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddSingleton<IConnectionMultiplexer>(options => ConnectionMultiplexer.Connect(configuration.GetConnectionString("Redis")));
            services.AddScoped<ISeriesRepo, RedisSeriesRepo>();

            return services;
        }
    }
}
