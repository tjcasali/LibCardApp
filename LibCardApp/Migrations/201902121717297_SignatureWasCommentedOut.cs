namespace LibCardApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class SignatureWasCommentedOut : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Patrons", "Signature", c => c.Binary(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Patrons", "Signature");
        }
    }
}
