using FluentMigrator;
using Movies.DataAccess.Dtos;

namespace Movies.API.Migrations
{
    [Migration(1000001)]
    public class InitialSeed_1000001 : Migration
    {
        public override void Down()
        {
            Delete.FromTable("Movies")
                .Row(new MovieDatabaseDto
                {
                    Name = "Infinite Azure",
                    Author = "Zafina",
                    Year = 2022
                });
        }

        public override void Up()
        {
            Insert.IntoTable("Movies")
                .Row(new MovieDatabaseDto
                {
                    Name = "Infinite Azure",
                    Author = "Zafina",
                    Year = 2022
                });
        }
    }
}
