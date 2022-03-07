using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Linq;
using Courses.API.Models;

namespace Courses.API.Data
{
    public static class PrepDb
    {
        public static void PrepPopulation(IApplicationBuilder app) //app to get service
        {
            using(var serviceScope = app.ApplicationServices.CreateScope()) //get context service to be used with seeddata
            {
                SeedData(serviceScope.ServiceProvider.GetService<DatabaseContext>());
            }
        }

        public static void SeedData(DatabaseContext context) //context needed for interacting with db
        {
            System.Console.WriteLine("Applying migrations...");

            context.Database.Migrate();

            if (!context.Course.Any())
            {
                System.Console.WriteLine("Adding data - seeding...");
                context.Course.AddRange(new Course
                {
                    Name = "Combo 1",
                    Duration = 10,
                    Price = 59
                },
                new Course
                {
                    Name = "Combo 2",
                    Duration = 25,
                    Price = 89
                },
                new Course
                {
                    Name = "Combo 3",
                    Duration = 30,
                    Price = 259
                }
                );
            }

            context.SaveChanges();
        }
    }
}
