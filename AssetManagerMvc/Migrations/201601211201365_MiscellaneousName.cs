namespace AssetManagerMvc.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class MiscellaneousName : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Assets", "MiscellaneousName", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Assets", "MiscellaneousName");
        }
    }
}
