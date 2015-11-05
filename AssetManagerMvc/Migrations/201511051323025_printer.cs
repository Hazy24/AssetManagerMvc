namespace AssetManagerMvc.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class printer : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Assets", "PrinterName", c => c.String());
            AddColumn("dbo.Assets", "TonerModel", c => c.String());
            AddColumn("dbo.Assets", "DrumModel", c => c.String());
            AddColumn("dbo.Assets", "IpAddress_Address", c => c.Long());
            AddColumn("dbo.Assets", "IpAddress_ScopeId", c => c.Long());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Assets", "IpAddress_ScopeId");
            DropColumn("dbo.Assets", "IpAddress_Address");
            DropColumn("dbo.Assets", "DrumModel");
            DropColumn("dbo.Assets", "TonerModel");
            DropColumn("dbo.Assets", "PrinterName");
        }
    }
}
