namespace Call.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Request_Question : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Requests", "Question", c => c.String(maxLength: 1024));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Requests", "Question");
        }
    }
}
