using FluentMigrator;

namespace SuperSchnell.CompanyDataProvider.DBMigrate.MigrationSteps
{
    [Migration(4)]
    public class MigrationStep004 : Migration
    {
        public override void Up()
        {
            Execute.Sql("TRUNCATE TABLE DanishCompany");
            Alter.Table("DanishCompany")
                .AddColumn("PlaceName").AsString().NotNullable()
                .AddColumn("CoName").AsString().NotNullable()
                .AddColumn("Email").AsString().NotNullable()
                .AddColumn("Phone").AsString().NotNullable();
        }

        public override void Down()
        {
            Delete.Column("PlaceName").FromTable("DanishCompany");
            Delete.Column("CoName").FromTable("DanishCompany");
            Delete.Column("Email").FromTable("DanishCompany");
            Delete.Column("Phone").FromTable("DanishCompany");
        }
    }
}