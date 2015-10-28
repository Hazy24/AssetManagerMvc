namespace AssetManagerMvc.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class modelnamerequiered : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Assets", "ModelName", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Assets", "ModelName", c => c.String());
        }
    }
}
