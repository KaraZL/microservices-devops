using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Series.API.Data;
using StackExchange.Redis;
using Polly;

namespace Series.Infrastructure
{
    public static class InfrastructureServiceRegistration
    {
        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
        {
            var retryRedis = Policy.Handle<RedisException>()
                                    .WaitAndRetry(
                                        retryCount: 5,
                                        sleepDurationProvider: retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)),
                                        onRetry: (exception, retryCount, context) =>
                                        {
                                            Console.WriteLine($"--Series.Infrastructure : Redis Connect Retry Policy... {retryCount}");
                                        });

            services.AddSingleton<IConnectionMultiplexer>(options => retryRedis.Execute(() => ConnectionMultiplexer.Connect(configuration.GetConnectionString("Redis"))));
            services.AddScoped<ISeriesRepo, RedisSeriesRepo>();

            return services;
        }
    }
}
