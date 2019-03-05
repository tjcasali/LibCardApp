namespace LibCardApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class MakeAllPatronFieldsRequired : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Patrons", "FirstName", c => c.String(nullable: false));
            AlterColumn("dbo.Patrons", "LastName", c => c.String(nullable: false));
            AlterColumn("dbo.Patrons", "MiddleInitial", c => c.String(nullable: false));
            AlterColumn("dbo.Patrons", "Address", c => c.String(nullable: false));
            AlterColumn("dbo.Patrons", "City", c => c.String(nullable: false));
            AlterColumn("dbo.Patrons", "State", c => c.String(nullable: false));
            AlterColumn("dbo.Patrons", "Email", c => c.String(nullable: false));
            AlterColumn("dbo.Patrons", "Phone", c => c.String(nullable: false));
            AlterColumn("dbo.Patrons", "PType", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Patrons", "PType", c => c.String());
            AlterColumn("dbo.Patrons", "Phone", c => c.String());
            AlterColumn("dbo.Patrons", "Email", c => c.String());
            AlterColumn("dbo.Patrons", "State", c => c.String());
            AlterColumn("dbo.Patrons", "City", c => c.String());
            AlterColumn("dbo.Patrons", "Address", c => c.String());
            AlterColumn("dbo.Patrons", "MiddleInitial", c => c.String());
            AlterColumn("dbo.Patrons", "LastName", c => c.String());
            AlterColumn("dbo.Patrons", "FirstName", c => c.String());
        }
    }
}
