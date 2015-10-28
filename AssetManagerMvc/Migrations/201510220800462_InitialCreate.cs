namespace AssetManagerMvc.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Assets",
                c => new
                    {
                        AssetId = c.Int(nullable: false, identity: true),
                        SerialNumber = c.String(),
                        ModelName = c.String(),
                        PurchaseDate = c.DateTime(),
                        PurchasePrice = c.Decimal(precision: 18, scale: 2),
                        Owner = c.String(),
                        Supplier = c.String(),
                        Manufacturer = c.String(),
                        ComputerName = c.String(),
                        ComputerType = c.String(),
                        OfficeVersion = c.String(),
                        OperatingSystem = c.String(),
                        Browser = c.String(),
                        AntiVirus = c.String(),
                        IsTeamViewerInstalled = c.Boolean(),
                        Discriminator = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => t.AssetId);
            
            CreateTable(
                "dbo.UsePeriods",
                c => new
                    {
                        UsePeriodId = c.Int(nullable: false, identity: true),
                        UserAccountId = c.Int(),
                        StartDate = c.DateTime(),
                        EndDate = c.DateTime(),
                        Remark = c.String(),
                        Function = c.String(),
                        UsePeriodStatusId = c.Int(),
                        AssetId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.UsePeriodId)
                .ForeignKey("dbo.Assets", t => t.AssetId, cascadeDelete: true)
                .ForeignKey("dbo.UsePeriodStatus", t => t.UsePeriodStatusId)
                .ForeignKey("dbo.UserAccounts", t => t.UserAccountId)
                .Index(t => t.UserAccountId)
                .Index(t => t.UsePeriodStatusId)
                .Index(t => t.AssetId);
            
            CreateTable(
                "dbo.UsePeriodStatus",
                c => new
                    {
                        UsePeriodStatusId = c.Int(nullable: false, identity: true),
                        Description = c.String(),
                        ColorCode = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.UsePeriodStatusId);
            
            CreateTable(
                "dbo.UserAccounts",
                c => new
                    {
                        UserAccountId = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        GivenName = c.String(),
                        UserPrincipalName = c.String(),
                        Sn = c.String(),
                        Mail = c.String(),
                        Company = c.String(),
                        Department = c.String(),
                        IsAdmin = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.UserAccountId);
            
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
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Assets", t => t.Asset_AssetId)
                .Index(t => t.Asset_AssetId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Incidents", "Asset_AssetId", "dbo.Assets");
            DropForeignKey("dbo.UsePeriods", "UserAccountId", "dbo.UserAccounts");
            DropForeignKey("dbo.UsePeriods", "UsePeriodStatusId", "dbo.UsePeriodStatus");
            DropForeignKey("dbo.UsePeriods", "AssetId", "dbo.Assets");
            DropIndex("dbo.Incidents", new[] { "Asset_AssetId" });
            DropIndex("dbo.UsePeriods", new[] { "AssetId" });
            DropIndex("dbo.UsePeriods", new[] { "UsePeriodStatusId" });
            DropIndex("dbo.UsePeriods", new[] { "UserAccountId" });
            DropTable("dbo.Incidents");
            DropTable("dbo.UserAccounts");
            DropTable("dbo.UsePeriodStatus");
            DropTable("dbo.UsePeriods");
            DropTable("dbo.Assets");
        }
    }
}
