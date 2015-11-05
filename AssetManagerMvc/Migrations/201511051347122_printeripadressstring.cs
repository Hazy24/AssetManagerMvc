namespace AssetManagerMvc.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class printeripadressstring : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Assets", "IpAddress", c => c.String());
            DropColumn("dbo.Assets", "IpAddress_Address");
            DropColumn("dbo.Assets", "IpAddress_ScopeId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Assets", "IpAddress_ScopeId", c => c.Long());
            AddColumn("dbo.Assets", "IpAddress_Address", c => c.Long());
            DropColumn("dbo.Assets", "IpAddress");
        }
    }
}
