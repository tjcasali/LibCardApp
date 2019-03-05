namespace LibCardApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedSignatureToDatabase : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Patrons", "Signature", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Patrons", "Signature");
        }
    }
}
