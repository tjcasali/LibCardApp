namespace LibCardApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DateSubmittedFromDateTimeBackToString : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Patrons", "DateSubmitted", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Patrons", "DateSubmitted", c => c.DateTime(nullable: false));
        }
    }
}
