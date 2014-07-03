namespace Talentify.ORM.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addevents : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.BasePage",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Title = c.String(),
                        Headline = c.String(),
                        PreviewImage = c.String(),
                        Image = c.String(),
                        Content = c.String(),
                        IsOnline = c.Boolean(nullable: false),
                        JoinedDate = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        UpdateDate = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        CreatedById = c.Int(nullable: false),
                        UpdateById = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.BaseUser", t => t.CreatedById)
                .ForeignKey("dbo.BaseUser", t => t.UpdateById)
                .Index(t => t.CreatedById)
                .Index(t => t.UpdateById);
            
            CreateTable(
                "dbo.Event",
                c => new
                    {
                        Id = c.Int(nullable: false),
                        JoinedDate = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        Address = c.String(),
                        ZipCode = c.String(),
                        City = c.String(),
                        Latitude = c.String(),
                        Longitude = c.String(),
                        MaxParticipant = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.BasePage", t => t.Id)
                .Index(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Event", "Id", "dbo.BasePage");
            DropForeignKey("dbo.BasePage", "UpdateById", "dbo.BaseUser");
            DropForeignKey("dbo.BasePage", "CreatedById", "dbo.BaseUser");
            DropIndex("dbo.Event", new[] { "Id" });
            DropIndex("dbo.BasePage", new[] { "UpdateById" });
            DropIndex("dbo.BasePage", new[] { "CreatedById" });
            DropTable("dbo.Event");
            DropTable("dbo.BasePage");
        }
    }
}
