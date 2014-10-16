namespace Talentify.ORM.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class schooloptional : DbMigration
    {
        public override void Up()
        {
            DropIndex("dbo.Student", new[] { "SchoolId" });
            AlterColumn("dbo.Student", "SchoolId", c => c.Int());
            CreateIndex("dbo.Student", "SchoolId");
        }
        
        public override void Down()
        {
            DropIndex("dbo.Student", new[] { "SchoolId" });
            AlterColumn("dbo.Student", "SchoolId", c => c.Int(nullable: false));
            CreateIndex("dbo.Student", "SchoolId");
        }
    }
}
