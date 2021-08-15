namespace projeto_dm106.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class orders : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Orders", "dataPostada", c => c.String());
            AddColumn("dbo.Orders", "dataEntrega", c => c.String());
            AddColumn("dbo.Orders", "status", c => c.String());
            AddColumn("dbo.Orders", "precoTotal", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AddColumn("dbo.Orders", "pesoTotal", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AddColumn("dbo.OrderItems", "NumProduct", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.OrderItems", "NumProduct");
            DropColumn("dbo.Orders", "pesoTotal");
            DropColumn("dbo.Orders", "precoTotal");
            DropColumn("dbo.Orders", "status");
            DropColumn("dbo.Orders", "dataEntrega");
            DropColumn("dbo.Orders", "dataPostada");
        }
    }
}
