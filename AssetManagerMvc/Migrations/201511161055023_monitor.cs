namespace AssetManagerMvc.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class monitor : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Assets", "Size", c => c.Single());
            AddColumn("dbo.Assets", "MaxResolution", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Assets", "MaxResolution");
            DropColumn("dbo.Assets", "Size");
        }
    }
}
