using FluentMigrator;

namespace Books.API.Migrations
{
    [Migration(206060601)] //Fluentmigrator.runner
    public class InitialTables_206060601 : Migration //FluentMigrator
    {
        public override void Down()
        {
            Delete.Table("Books");
        }

        public override void Up()
        {
            Create.Table("Books")
                .WithColumn("Id").AsInt32().NotNullable().PrimaryKey().Identity() //Identity permet l'auto increment
                .WithColumn("Title").AsString(50).NotNullable()
                .WithColumn("Author").AsString(50).NotNullable()
                .WithColumn("Date").AsDateTime().NotNullable();
        }
    }
}
