namespace LibCardApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AdjustedPType : DbMigration
    {
        public override void Up()
        {
            RenameColumn(table: "dbo.Patrons", name: "PType_PTypeID", newName: "PTypeId");
            RenameIndex(table: "dbo.Patrons", name: "IX_PType_PTypeID", newName: "IX_PTypeId");
        }
        
        public override void Down()
        {
            RenameIndex(table: "dbo.Patrons", name: "IX_PTypeId", newName: "IX_PType_PTypeID");
            RenameColumn(table: "dbo.Patrons", name: "PTypeId", newName: "PType_PTypeID");
        }
    }
}
