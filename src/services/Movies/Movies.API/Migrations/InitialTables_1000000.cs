using FluentMigrator;

namespace Movies.API.Migrations
{
    [Migration(1000000)]
    public class InitialTables_1000000 : Migration
    {
        public override void Down()
        {
            Delete.Table("Movies");
        }

        public override void Up()
        {
            Create.Table("Movies")
                .WithColumn("Id").AsInt32().PrimaryKey().Identity()
                .WithColumn("Name").AsString()
                .WithColumn("Author").AsString()
                .WithColumn("Year").AsInt32();
        }
    }
}
