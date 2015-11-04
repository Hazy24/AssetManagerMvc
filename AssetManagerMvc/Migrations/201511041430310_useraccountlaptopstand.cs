namespace AssetManagerMvc.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class useraccountlaptopstand : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.UserAccounts", "LaptopStand", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.UserAccounts", "LaptopStand");
        }
    }
}
