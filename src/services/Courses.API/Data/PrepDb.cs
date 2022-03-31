using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Linq;
using System;
using Courses.API.Models;
using Polly;
using Microsoft.Data.SqlClient;
using Courses.API.Policies;

namespace Courses.API.Data
{
    public static class PrepDb
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
            Console.WriteLine("Applying migrations...");

            policy.MigrationRetryPolicy.Execute(() => context.Database.Migrate());


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
