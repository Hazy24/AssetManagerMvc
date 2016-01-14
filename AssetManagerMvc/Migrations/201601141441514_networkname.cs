namespace AssetManagerMvc.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class networkname : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Assets", "NetworkName", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Assets", "NetworkName");
        }
    }
}
