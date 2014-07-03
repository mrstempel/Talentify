namespace Talentify.ORM.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addeventregistration : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.EventRegistration",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.Int(nullable: false),
                        EventId = c.Int(nullable: false),
                        CreatedDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Event", t => t.EventId)
                .ForeignKey("dbo.BaseUser", t => t.UserId)
                .Index(t => t.UserId)
                .Index(t => t.EventId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.EventRegistration", "UserId", "dbo.BaseUser");
            DropForeignKey("dbo.EventRegistration", "EventId", "dbo.Event");
            DropIndex("dbo.EventRegistration", new[] { "EventId" });
            DropIndex("dbo.EventRegistration", new[] { "UserId" });
            DropTable("dbo.EventRegistration");
        }
    }
}
