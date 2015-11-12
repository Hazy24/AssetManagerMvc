namespace AssetManagerMvc.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class beamer : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Assets", "BeamerName", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Assets", "BeamerName");
        }
    }
}
