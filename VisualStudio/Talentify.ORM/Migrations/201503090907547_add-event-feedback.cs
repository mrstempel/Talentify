namespace Talentify.ORM.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addeventfeedback : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.EventFeedback",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.Int(nullable: false),
                        Liked = c.Int(nullable: false),
                        Helpful = c.Int(nullable: false),
                        RecommendWorthy = c.Boolean(nullable: false),
                        Comments = c.String(),
                        CreatedDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.BaseUser", t => t.UserId)
                .Index(t => t.UserId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.EventFeedback", "UserId", "dbo.BaseUser");
            DropIndex("dbo.EventFeedback", new[] { "UserId" });
            DropTable("dbo.EventFeedback");
        }
    }
}
