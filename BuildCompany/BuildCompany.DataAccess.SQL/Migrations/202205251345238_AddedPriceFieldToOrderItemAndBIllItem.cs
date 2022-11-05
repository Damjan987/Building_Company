namespace BuildCompany.DataAccess.SQL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedPriceFieldToOrderItemAndBIllItem : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.BillItems", "Price", c => c.Int(nullable: false));
            AddColumn("dbo.ItemInOrders", "Price", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.ItemInOrders", "Price");
            DropColumn("dbo.BillItems", "Price");
        }
    }
}
