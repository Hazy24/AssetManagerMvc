namespace AssetManagerMvc.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class removeincident : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Incidents", "Asset_AssetId", "dbo.Assets");
            DropIndex("dbo.Incidents", new[] { "Asset_AssetId" });
            DropTable("dbo.Incidents");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.Incidents",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Description = c.String(),
                        Date = c.DateTime(nullable: false),
                        Status = c.String(),
                        Asset_AssetId = c.Int(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateIndex("dbo.Incidents", "Asset_AssetId");
            AddForeignKey("dbo.Incidents", "Asset_AssetId", "dbo.Assets", "AssetId");
        }
    }
}
