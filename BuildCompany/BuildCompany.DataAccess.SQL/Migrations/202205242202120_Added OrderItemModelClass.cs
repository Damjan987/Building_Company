namespace BuildCompany.DataAccess.SQL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedOrderItemModelClass : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ItemInOrders",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Order = c.String(),
                        Item = c.String(),
                        Quantity = c.Int(nullable: false),
                        CreatedAt = c.DateTimeOffset(nullable: false, precision: 7),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.ItemInOrders");
        }
    }
}
