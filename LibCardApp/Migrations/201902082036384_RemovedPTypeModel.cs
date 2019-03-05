namespace LibCardApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RemovedPTypeModel : DbMigration
    {
        public override void Up()
        {
            //DropForeignKey("dbo.Patrons", "PTypeId", "dbo.PTypes");
            //DropIndex("dbo.Patrons", new[] { "PTypeId" });
            //AddColumn("dbo.Patrons", "PType", c => c.String(nullable: false));
            //DropColumn("dbo.Patrons", "PTypeId");
        }
        
        public override void Down()
        {
            //CreateTable(
            //    "dbo.PTypes",
            //    c => new
            //        {
            //            PTypeId = c.String(nullable: false, maxLength: 128),
            //            AdultYAChild = c.String(),
            //            Birthdate = c.DateTime(nullable: false),
            //            InternetAccess = c.Boolean(nullable: false),
            //        })
            //    .PrimaryKey(t => t.PTypeId);
            
            //AddColumn("dbo.Patrons", "PTypeId", c => c.String(nullable: false, maxLength: 128));
            //DropColumn("dbo.Patrons", "PType");
            //CreateIndex("dbo.Patrons", "PTypeId");
            //AddForeignKey("dbo.Patrons", "PTypeId", "dbo.PTypes", "PTypeId", cascadeDelete: true);
        }
    }
}
