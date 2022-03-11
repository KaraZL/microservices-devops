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
                SeedData(serviceScope.ServiceProvider.GetService<DatabaseContext>());
            }
        }

        public static void SeedData(DatabaseContext context) //context needed for interacting with db
        {
            System.Console.WriteLine("Applying migrations...");

            context.Database.Migrate();

            context.SaveChanges();
        }
    }
}
