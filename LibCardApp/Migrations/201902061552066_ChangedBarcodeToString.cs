namespace LibCardApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChangedBarcodeToString : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Patrons", "Barcode", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Patrons", "Barcode", c => c.Single(nullable: false));
        }
    }
}
