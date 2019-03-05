namespace LibCardApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DateSubmittedFromStringToDateTime : DbMigration
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
