namespace AssetManagerMvc.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class serialnumberrequiered : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Assets", "SerialNumber", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Assets", "SerialNumber", c => c.String());
        }
    }
}
