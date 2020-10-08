namespace Call.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class User_Phone : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AbpUsers", "Phone", c => c.String(maxLength: 12));
        }
        
        public override void Down()
        {
            DropColumn("dbo.AbpUsers", "Phone");
        }
    }
}
