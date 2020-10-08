namespace Call.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Request_Time : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Requests", "TreatedTime", c => c.Time(nullable: false, precision: 7));
            AddColumn("dbo.Requests", "AnsweredTime", c => c.Time(nullable: false, precision: 7));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Requests", "AnsweredTime");
            DropColumn("dbo.Requests", "TreatedTime");
        }
    }
}
