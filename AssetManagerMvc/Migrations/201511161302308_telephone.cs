namespace AssetManagerMvc.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class telephone : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Assets", "TelephoneType", c => c.String());
            AddColumn("dbo.Assets", "Number", c => c.String());
            AddColumn("dbo.Assets", "NumberIntern", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Assets", "NumberIntern");
            DropColumn("dbo.Assets", "Number");
            DropColumn("dbo.Assets", "TelephoneType");
        }
    }
}
