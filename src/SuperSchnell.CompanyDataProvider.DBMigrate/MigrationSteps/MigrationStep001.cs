using FluentMigrator;

namespace SuperSchnell.CompanyDataProvider.DBMigrate.MigrationSteps
{
    [Migration(1)]
    public class MigrationStep001:Migration
    {
        public override void Up()
        {
            Create.Table("DanishCompany")
                  .WithColumn("Id").AsInt64().PrimaryKey()
                  .WithColumn("[Version]").AsInt32().NotNullable()
                  .WithColumn("CompanyName").AsString().NotNullable()
                  .WithColumn("CVRNumber").AsString().NotNullable()
                  .WithColumn("Street").AsString().NotNullable()
                  .WithColumn("Zip").AsString().NotNullable()
                  .WithColumn("City").AsString().NotNullable();

        }

        public override void Down()
        {
            Delete.Table("DanishCompany");
        }
    }
}
