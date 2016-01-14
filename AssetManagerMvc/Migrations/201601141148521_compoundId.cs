namespace AssetManagerMvc.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class compoundId : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Assets", "CompoundId", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Assets", "CompoundId");
        }
    }
}
