namespace Talentify.ORM.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class teachersubjects : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.SubjectCategory", "Teacher_Id", "dbo.Teacher");
            DropIndex("dbo.SubjectCategory", new[] { "Teacher_Id" });
            CreateTable(
                "dbo.SubjectCategoriesToTeachers",
                c => new
                    {
                        Teacher_Id = c.Int(nullable: false),
                        SubjectCategory_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Teacher_Id, t.SubjectCategory_Id })
                .ForeignKey("dbo.Teacher", t => t.Teacher_Id, cascadeDelete: true)
                .ForeignKey("dbo.SubjectCategory", t => t.SubjectCategory_Id, cascadeDelete: true)
                .Index(t => t.Teacher_Id)
                .Index(t => t.SubjectCategory_Id);
            
            DropColumn("dbo.SubjectCategory", "Teacher_Id");
        }
        
        public override void Down()
        {
            AddColumn("dbo.SubjectCategory", "Teacher_Id", c => c.Int());
            DropForeignKey("dbo.SubjectCategoriesToTeachers", "SubjectCategory_Id", "dbo.SubjectCategory");
            DropForeignKey("dbo.SubjectCategoriesToTeachers", "Teacher_Id", "dbo.Teacher");
            DropIndex("dbo.SubjectCategoriesToTeachers", new[] { "SubjectCategory_Id" });
            DropIndex("dbo.SubjectCategoriesToTeachers", new[] { "Teacher_Id" });
            DropTable("dbo.SubjectCategoriesToTeachers");
            CreateIndex("dbo.SubjectCategory", "Teacher_Id");
            AddForeignKey("dbo.SubjectCategory", "Teacher_Id", "dbo.Teacher", "Id");
        }
    }
}
