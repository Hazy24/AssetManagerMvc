namespace AssetManagerMvc.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class useraccountdepartment : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.UserAccounts", "DepartmentId", c => c.Int());
            CreateIndex("dbo.UserAccounts", "DepartmentId");
            AddForeignKey("dbo.UserAccounts", "DepartmentId", "dbo.Departments", "DepartmentId");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.UserAccounts", "DepartmentId", "dbo.Departments");
            DropIndex("dbo.UserAccounts", new[] { "DepartmentId" });
            DropColumn("dbo.UserAccounts", "DepartmentId");
        }
    }
}
