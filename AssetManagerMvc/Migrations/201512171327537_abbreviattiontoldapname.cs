namespace AssetManagerMvc.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class abbreviattiontoldapname : DbMigration
    {
        public override void Up()
        {
            RenameColumn("dbo.Departments", "Abbreviation", "LdapName");            
        }
        
        public override void Down()
        {
            RenameColumn("dbo.Departments", "LdapName", "Abbreviation");
        }
    }
}
