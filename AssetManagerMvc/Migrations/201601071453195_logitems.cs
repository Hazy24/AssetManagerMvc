namespace AssetManagerMvc.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class logitems : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.LogItems",
                c => new
                    {
                        LogItemId = c.Int(nullable: false, identity: true),
                        Text = c.String(),
                        DateCreated = c.DateTime(nullable: false),
                        AssetId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.LogItemId)
                .ForeignKey("dbo.Assets", t => t.AssetId, cascadeDelete: true)
                .Index(t => t.AssetId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.LogItems", "AssetId", "dbo.Assets");
            DropIndex("dbo.LogItems", new[] { "AssetId" });
            DropTable("dbo.LogItems");
        }
    }
}
