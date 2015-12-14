namespace AssetManagerMvc.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class patchpoint : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.PatchPoints",
                c => new
                    {
                        PatchPointId = c.Int(nullable: false, identity: true),
                        Number = c.Int(nullable: false),
                        Floor = c.Int(nullable: false),
                        RoomName = c.String(),
                        RoomNumber = c.Int(nullable: false),
                        Tile = c.String(),
                        Remark = c.String(),
                        Function = c.String(),
                    })
                .PrimaryKey(t => t.PatchPointId);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.PatchPoints");
        }
    }
}
