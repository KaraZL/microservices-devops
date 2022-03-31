using GeneralStore.API.Policies;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace GeneralStore.API.Data
{
    public class PrepDb
    {
        public static void PrepPopulation(IApplicationBuilder app) //app to get service
        {
            using (var serviceScope = app.ApplicationServices.CreateScope()) //get context service to be used with seeddata
            {
                var policy = serviceScope.ServiceProvider.GetService<ClientPolicy>();
                var context = serviceScope.ServiceProvider.GetService<DatabaseContext>();
                SeedData(context, policy);
            }
        }

        public static void SeedData(DatabaseContext context, ClientPolicy policy) //context needed for interacting with db
        {
            System.Console.WriteLine("Applying migrations...");

            policy.MigrationRetryPolicy.Execute(() => context.Database.Migrate());

            context.SaveChanges();
        }
    }
}
