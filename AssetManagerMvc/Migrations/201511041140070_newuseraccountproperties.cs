namespace AssetManagerMvc.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class newuseraccountproperties : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.UserAccounts", "Remark", c => c.String());
            AddColumn("dbo.UserAccounts", "Headset", c => c.Int(nullable: false));
            AddColumn("dbo.UserAccounts", "Speakers", c => c.Int(nullable: false));
            AddColumn("dbo.UserAccounts", "Keyboard", c => c.Int(nullable: false));
            AddColumn("dbo.UserAccounts", "Mouse", c => c.Int(nullable: false));
            AddColumn("dbo.UserAccounts", "WirelessMouse", c => c.Int(nullable: false));
            AddColumn("dbo.UserAccounts", "UsbStick", c => c.Int(nullable: false));
            AddColumn("dbo.UserAccounts", "LaptopBag", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.UserAccounts", "LaptopBag");
            DropColumn("dbo.UserAccounts", "UsbStick");
            DropColumn("dbo.UserAccounts", "WirelessMouse");
            DropColumn("dbo.UserAccounts", "Mouse");
            DropColumn("dbo.UserAccounts", "Keyboard");
            DropColumn("dbo.UserAccounts", "Speakers");
            DropColumn("dbo.UserAccounts", "Headset");
            DropColumn("dbo.UserAccounts", "Remark");
        }
    }
}
