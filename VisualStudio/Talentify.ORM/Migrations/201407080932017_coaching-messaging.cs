namespace Talentify.ORM.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class coachingmessaging : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.CoachingRequest",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        FromUserId = c.Int(nullable: false),
                        ToUserId = c.Int(nullable: false),
                        SubjectCategoryId = c.Int(nullable: false),
                        Class = c.Int(nullable: false),
                        Price = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Date = c.DateTime(precision: 7, storeType: "datetime2"),
                        Duration = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Conversation_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Conversation", t => t.Conversation_Id)
                .ForeignKey("dbo.Student", t => t.FromUserId)
                .ForeignKey("dbo.SubjectCategory", t => t.SubjectCategoryId)
                .ForeignKey("dbo.Student", t => t.ToUserId)
                .Index(t => t.FromUserId)
                .Index(t => t.ToUserId)
                .Index(t => t.SubjectCategoryId)
                .Index(t => t.Conversation_Id);
            
            CreateTable(
                "dbo.Conversation",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Subject = c.String(),
                        ConversationType = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Message",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ConversationId = c.Int(nullable: false),
                        FromUserId = c.Int(nullable: false),
                        CreatedDate = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        Text = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Conversation", t => t.ConversationId)
                .ForeignKey("dbo.BaseUser", t => t.FromUserId)
                .Index(t => t.ConversationId)
                .Index(t => t.FromUserId);
            
            CreateTable(
                "dbo.MessageRecipient",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.Int(nullable: false),
                        ReadTime = c.DateTime(precision: 7, storeType: "datetime2"),
                        Message_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.BaseUser", t => t.UserId)
                .ForeignKey("dbo.Message", t => t.Message_Id)
                .Index(t => t.UserId)
                .Index(t => t.Message_Id);
            
            CreateTable(
                "dbo.CoachingRatings",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        CoachingRequestId = c.Int(nullable: false),
                        FromUserId = c.Int(nullable: false),
                        ToUserId = c.Int(nullable: false),
                        RatingType = c.Int(nullable: false),
                        Value = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.CoachingRequest", t => t.CoachingRequestId)
                .ForeignKey("dbo.Student", t => t.FromUserId)
                .ForeignKey("dbo.Student", t => t.ToUserId)
                .Index(t => t.CoachingRequestId)
                .Index(t => t.FromUserId)
                .Index(t => t.ToUserId);
            
            CreateTable(
                "dbo.CoachingRequestStatus",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        CoachingRequestId = c.Int(nullable: false),
                        CreatedById = c.Int(nullable: false),
                        StatusType = c.Int(nullable: false),
                        Text = c.String(),
                        CreatedDate = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.CoachingRequest", t => t.CoachingRequestId)
                .ForeignKey("dbo.BaseUser", t => t.CreatedById)
                .Index(t => t.CoachingRequestId)
                .Index(t => t.CreatedById);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.CoachingRequest", "ToUserId", "dbo.Student");
            DropForeignKey("dbo.CoachingRequest", "SubjectCategoryId", "dbo.SubjectCategory");
            DropForeignKey("dbo.CoachingRequestStatus", "CreatedById", "dbo.BaseUser");
            DropForeignKey("dbo.CoachingRequestStatus", "CoachingRequestId", "dbo.CoachingRequest");
            DropForeignKey("dbo.CoachingRatings", "ToUserId", "dbo.Student");
            DropForeignKey("dbo.CoachingRatings", "FromUserId", "dbo.Student");
            DropForeignKey("dbo.CoachingRatings", "CoachingRequestId", "dbo.CoachingRequest");
            DropForeignKey("dbo.CoachingRequest", "FromUserId", "dbo.Student");
            DropForeignKey("dbo.CoachingRequest", "Conversation_Id", "dbo.Conversation");
            DropForeignKey("dbo.MessageRecipient", "Message_Id", "dbo.Message");
            DropForeignKey("dbo.MessageRecipient", "UserId", "dbo.BaseUser");
            DropForeignKey("dbo.Message", "FromUserId", "dbo.BaseUser");
            DropForeignKey("dbo.Message", "ConversationId", "dbo.Conversation");
            DropIndex("dbo.CoachingRequestStatus", new[] { "CreatedById" });
            DropIndex("dbo.CoachingRequestStatus", new[] { "CoachingRequestId" });
            DropIndex("dbo.CoachingRatings", new[] { "ToUserId" });
            DropIndex("dbo.CoachingRatings", new[] { "FromUserId" });
            DropIndex("dbo.CoachingRatings", new[] { "CoachingRequestId" });
            DropIndex("dbo.MessageRecipient", new[] { "Message_Id" });
            DropIndex("dbo.MessageRecipient", new[] { "UserId" });
            DropIndex("dbo.Message", new[] { "FromUserId" });
            DropIndex("dbo.Message", new[] { "ConversationId" });
            DropIndex("dbo.CoachingRequest", new[] { "Conversation_Id" });
            DropIndex("dbo.CoachingRequest", new[] { "SubjectCategoryId" });
            DropIndex("dbo.CoachingRequest", new[] { "ToUserId" });
            DropIndex("dbo.CoachingRequest", new[] { "FromUserId" });
            DropTable("dbo.CoachingRequestStatus");
            DropTable("dbo.CoachingRatings");
            DropTable("dbo.MessageRecipient");
            DropTable("dbo.Message");
            DropTable("dbo.Conversation");
            DropTable("dbo.CoachingRequest");
        }
    }
}
