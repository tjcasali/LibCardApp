namespace LibCardApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class SecondDay : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Patrons", "MiddleInitial", c => c.String());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Patrons", "MiddleInitial", c => c.String(nullable: false));
        }
    }
}
