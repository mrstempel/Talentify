namespace Talentify.ORM.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addcoachingoffer : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.CoachingOffer",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.Int(nullable: false),
                        CreatedDate = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        UpdateDate = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        FromClass = c.Int(nullable: false),
                        ToClass = c.Int(nullable: false),
                        SubjectCategory_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.SubjectCategory", t => t.SubjectCategory_Id)
                .ForeignKey("dbo.Student", t => t.UserId)
                .Index(t => t.UserId)
                .Index(t => t.SubjectCategory_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.CoachingOffer", "UserId", "dbo.Student");
            DropForeignKey("dbo.CoachingOffer", "SubjectCategory_Id", "dbo.SubjectCategory");
            DropIndex("dbo.CoachingOffer", new[] { "SubjectCategory_Id" });
            DropIndex("dbo.CoachingOffer", new[] { "UserId" });
            DropTable("dbo.CoachingOffer");
        }
    }
}
