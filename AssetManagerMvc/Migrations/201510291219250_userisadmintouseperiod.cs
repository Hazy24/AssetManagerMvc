namespace AssetManagerMvc.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class userisadmintouseperiod : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.UsePeriods", "UserIsAdmin", c => c.Boolean(nullable: false));
            DropColumn("dbo.UserAccounts", "IsAdmin");
        }
        
        public override void Down()
        {
            AddColumn("dbo.UserAccounts", "IsAdmin", c => c.Boolean(nullable: false));
            DropColumn("dbo.UsePeriods", "UserIsAdmin");
        }
    }
}
