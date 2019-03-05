namespace LibCardApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RemovedRequiredOnEmailAndPhone : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Patrons", "Email", c => c.String());
            AlterColumn("dbo.Patrons", "Phone", c => c.String());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Patrons", "Phone", c => c.String(nullable: false));
            AlterColumn("dbo.Patrons", "Email", c => c.String(nullable: false));
        }
    }
}
