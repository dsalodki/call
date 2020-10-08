namespace Call.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Request_User : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Requests", "UserId", c => c.Long());
            CreateIndex("dbo.Requests", "UserId");
            AddForeignKey("dbo.Requests", "UserId", "dbo.AbpUsers", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Requests", "UserId", "dbo.AbpUsers");
            DropIndex("dbo.Requests", new[] { "UserId" });
            DropColumn("dbo.Requests", "UserId");
        }
    }
}
