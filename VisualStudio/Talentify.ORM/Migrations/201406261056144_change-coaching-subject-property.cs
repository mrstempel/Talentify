namespace Talentify.ORM.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class changecoachingsubjectproperty : DbMigration
    {
        public override void Up()
        {
            DropIndex("dbo.CoachingOffer", new[] { "SubjectCategory_Id" });
            RenameColumn(table: "dbo.CoachingOffer", name: "SubjectCategory_Id", newName: "SubjectCategoryId");
            AlterColumn("dbo.CoachingOffer", "SubjectCategoryId", c => c.Int(nullable: false));
            CreateIndex("dbo.CoachingOffer", "SubjectCategoryId");
        }
        
        public override void Down()
        {
            DropIndex("dbo.CoachingOffer", new[] { "SubjectCategoryId" });
            AlterColumn("dbo.CoachingOffer", "SubjectCategoryId", c => c.Int());
            RenameColumn(table: "dbo.CoachingOffer", name: "SubjectCategoryId", newName: "SubjectCategory_Id");
            CreateIndex("dbo.CoachingOffer", "SubjectCategory_Id");
        }
    }
}
