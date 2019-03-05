namespace LibCardApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DateSubmittedFromStringBackToDateTime : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Patrons", "DateSubmitted", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Patrons", "DateSubmitted", c => c.String(nullable: false));
        }
    }
}
