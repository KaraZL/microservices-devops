using Books.API.Migrations;
using FluentMigrator.Runner;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace Books.API.Data
{
    public class PrepDb
    {
        public static void PrepPopulation(IApplicationBuilder app) //app to get service
        {
            using (var serviceScope = app.ApplicationServices.CreateScope())
            {
                var databaseSvc = serviceScope.ServiceProvider.GetService<DatabaseSeed>();
                var fluentSvc = serviceScope.ServiceProvider.GetService<IMigrationRunner>();

                databaseSvc.CreateDatabase(); //créé la bdd
                fluentSvc.ListMigrations(); //affiche la liste des migrations dans la console
                fluentSvc.MigrateUp(); //crée les tables et ajoute les données
                databaseSvc.StoredProcedures();
            }

        }
    }
}
