namespace BuildCompany.DataAccess.SQL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedPriceFieldToCompanyItem : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.CompanyItems", "Price", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.CompanyItems", "Price");
        }
    }
}
