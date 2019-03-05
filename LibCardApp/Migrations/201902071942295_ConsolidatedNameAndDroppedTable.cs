namespace LibCardApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ConsolidatedNameAndDroppedTable : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Patrons", "Name", c => c.String(nullable: false));
            DropColumn("dbo.Patrons", "FirstName");
            DropColumn("dbo.Patrons", "LastName");
            DropColumn("dbo.Patrons", "MiddleInitial");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Patrons", "MiddleInitial", c => c.String());
            AddColumn("dbo.Patrons", "LastName", c => c.String(nullable: false));
            AddColumn("dbo.Patrons", "FirstName", c => c.String(nullable: false));
            DropColumn("dbo.Patrons", "Name");
        }
    }
}
