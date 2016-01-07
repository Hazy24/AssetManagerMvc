namespace AssetManagerMvc.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class imageversionqualitycheck : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Assets", "ImageVersion", c => c.String());
            AddColumn("dbo.Assets", "QualityCheck", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Assets", "QualityCheck");
            DropColumn("dbo.Assets", "ImageVersion");
        }
    }
}
