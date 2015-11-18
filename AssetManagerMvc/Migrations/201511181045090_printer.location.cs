namespace AssetManagerMvc.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class printerlocation : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Assets", "Location", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Assets", "Location");
        }
    }
}
