namespace Call.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Request : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Requests",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        TenantId = c.Int(nullable: false),
                        Phone = c.String(nullable: false),
                        Name = c.String(nullable: false),
                        Email = c.String(),
                        Created = c.DateTime(nullable: false),
                        State = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AbpTenants", t => t.TenantId, cascadeDelete: true)
                .Index(t => t.TenantId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Requests", "TenantId", "dbo.AbpTenants");
            DropIndex("dbo.Requests", new[] { "TenantId" });
            DropTable("dbo.Requests");
        }
    }
}
