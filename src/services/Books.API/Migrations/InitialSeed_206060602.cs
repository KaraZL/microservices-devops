using Books.API.Dtos;
using FluentMigrator;

namespace Books.API.Migrations
{
    [Migration(206060602)]
    public class InitialSeed_206060602 : Migration
    {
        public override void Down()
        {
            Delete.FromTable("Books")
                .Row(new BookMigrationDto
                {
                    Title = "Zafina Tactics",
                    Date = System.DateTime.Now,
                    Author = "Zafina"
                });

            Delete.FromTable("Books")
                .Row(new BookMigrationDto
                {
                    Title = "Zafina Tactics v2",
                    Date = System.DateTime.Now.AddDays(-1),
                    Author = "Zafina"
                });
        }

        public override void Up()
        {
            Insert.IntoTable("Books")
                .Row(new BookMigrationDto
                {
                    Title = "Zafina Tactics",
                    Date = System.DateTime.Now,
                    Author = "Zafina"
                });

            Insert.IntoTable("Books")
                .Row(new BookMigrationDto
                {
                    Title = "Zafina Tactics v2",
                    Date = System.DateTime.Now.AddDays(-1),
                    Author = "Zafina"
                });
        }
    }
}
