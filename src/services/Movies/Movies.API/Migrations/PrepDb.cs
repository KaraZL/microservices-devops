using FluentMigrator.Runner;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace Movies.API.Migrations
{
    public static class PrepDb
    {
        public static void Migrate(IApplicationBuilder app)
        {
            using (var serviceScope = app.ApplicationServices.CreateScope())
            {
                var seedSvc = serviceScope.ServiceProvider.GetRequiredService<DatabaseSeed>();
                var fluentSvc = serviceScope.ServiceProvider.GetRequiredService<IMigrationRunner>();

                seedSvc.CreateDatabase();
                fluentSvc.MigrateUp();
            }
        }
    }
}
