namespace Talentify.ORM.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addadminuser : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Admin",
                c => new
                    {
                        Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.BaseUser", t => t.Id)
                .Index(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Admin", "Id", "dbo.BaseUser");
            DropIndex("dbo.Admin", new[] { "Id" });
            DropTable("dbo.Admin");
        }
    }
}
