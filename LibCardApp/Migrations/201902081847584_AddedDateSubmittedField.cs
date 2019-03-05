namespace LibCardApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedDateSubmittedField : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Patrons", "DateSubmitted", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Patrons", "DateSubmitted");
        }
    }
}
