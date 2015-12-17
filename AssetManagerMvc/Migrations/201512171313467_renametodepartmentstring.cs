namespace AssetManagerMvc.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class renametodepartmentstring : DbMigration
    {
        public override void Up()
        {
            RenameColumn("dbo.UserAccounts", "Department", "DepartmentString");            
        }
        
        public override void Down()
        {
            RenameColumn("dbo.UserAccounts", "DepartmentString", "Department");
        }
    }
}
