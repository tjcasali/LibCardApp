namespace LibCardApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedPTypeModelAndChangedViewModel : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.PTypes",
                c => new
                    {
                        PTypeID = c.String(nullable: false, maxLength: 128),
                        AdultYAChild = c.String(),
                        Birthdate = c.DateTime(nullable: false),
                        InternetAccess = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.PTypeID);
            
            AddColumn("dbo.Patrons", "PType_PTypeID", c => c.String(nullable: false, maxLength: 128));
            CreateIndex("dbo.Patrons", "PType_PTypeID");
            AddForeignKey("dbo.Patrons", "PType_PTypeID", "dbo.PTypes", "PTypeID", cascadeDelete: true);
            DropColumn("dbo.Patrons", "PType");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Patrons", "PType", c => c.String(nullable: false));
            DropForeignKey("dbo.Patrons", "PType_PTypeID", "dbo.PTypes");
            DropIndex("dbo.Patrons", new[] { "PType_PTypeID" });
            DropColumn("dbo.Patrons", "PType_PTypeID");
            DropTable("dbo.PTypes");
        }
    }
}
