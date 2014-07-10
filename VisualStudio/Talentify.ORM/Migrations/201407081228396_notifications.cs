namespace Talentify.ORM.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class notifications : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Notification",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ToUserId = c.Int(nullable: false),
                        SenderId = c.Int(nullable: false),
                        CreatedDate = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        ReadDate = c.DateTime(precision: 7, storeType: "datetime2"),
                        Bonus = c.Int(nullable: false),
                        SenderType = c.Int(nullable: false),
                        IconType = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.BaseUser", t => t.ToUserId)
                .Index(t => t.ToUserId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Notification", "ToUserId", "dbo.BaseUser");
            DropIndex("dbo.Notification", new[] { "ToUserId" });
            DropTable("dbo.Notification");
        }
    }
}
