using FluentMigrator;

namespace SuperSchnell.CompanyDataProvider.DBMigrate.MigrationSteps
{
    [Migration(3)]
    public class MigrationStep003 : Migration
    {
        public override void Up()
        {
            Alter.Table("DanishCompany")
                .AlterColumn("CompanyName").AsString(10000).NotNullable();
        }

        public override void Down()
        {
        }
    }
}