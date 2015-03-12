namespace Talentify.ORM.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addeventidtoeventfeedback : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.EventFeedback", "EventId", c => c.Int(nullable: false));
            CreateIndex("dbo.EventFeedback", "EventId");
            AddForeignKey("dbo.EventFeedback", "EventId", "dbo.Event", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.EventFeedback", "EventId", "dbo.Event");
            DropIndex("dbo.EventFeedback", new[] { "EventId" });
            DropColumn("dbo.EventFeedback", "EventId");
        }
    }
}
