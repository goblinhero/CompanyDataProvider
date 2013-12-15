using FluentMigrator;

namespace SuperSchnell.CompanyDataProvider.DBMigrate.MigrationSteps
{
    [Migration(2)]
    public class MigrationStep002 : Migration
    {
        public override void Up()
        {
            Create.Table("hibernate_unique_key")
                .WithColumn("next_hi").AsInt64().Nullable();
            Execute.Sql("INSERT INTO hibernate_unique_key (next_hi) VALUES (1)");
        }

        public override void Down()
        {
            Delete.Table("hibernate_unique_key");
        }
    }
}