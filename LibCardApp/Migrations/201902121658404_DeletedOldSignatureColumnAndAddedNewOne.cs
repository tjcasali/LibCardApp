namespace LibCardApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DeletedOldSignatureColumnAndAddedNewOne : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Patrons", "Signature");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Patrons", "Signature", c => c.Binary(nullable: false));
        }
    }
}
