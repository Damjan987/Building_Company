namespace BuildCompany.DataAccess.SQL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedNameFieldToAllRelationalItemTables : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.BillItems", "Name", c => c.String());
            AddColumn("dbo.CompanyItems", "Name", c => c.String());
            AddColumn("dbo.ItemInOrders", "Name", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.ItemInOrders", "Name");
            DropColumn("dbo.CompanyItems", "Name");
            DropColumn("dbo.BillItems", "Name");
        }
    }
}
