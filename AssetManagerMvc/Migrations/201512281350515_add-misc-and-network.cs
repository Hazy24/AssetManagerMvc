namespace AssetManagerMvc.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addmiscandnetwork : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Assets", "MiscellaneousType", c => c.String());
            AddColumn("dbo.Assets", "NetworkType", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Assets", "NetworkType");
            DropColumn("dbo.Assets", "MiscellaneousType");
        }
    }
}
