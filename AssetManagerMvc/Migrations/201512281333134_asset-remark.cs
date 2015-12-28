namespace AssetManagerMvc.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class assetremark : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Assets", "Remark", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Assets", "Remark");
        }
    }
}
