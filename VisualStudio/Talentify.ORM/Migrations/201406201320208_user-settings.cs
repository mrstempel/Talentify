namespace Talentify.ORM.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class usersettings : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.UserSettings",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        HasNotifications = c.Boolean(nullable: false),
                        HasNewsletter = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            AddColumn("dbo.BaseUser", "SettingsId", c => c.Int(nullable: true));
            CreateIndex("dbo.BaseUser", "SettingsId");
            AddForeignKey("dbo.BaseUser", "SettingsId", "dbo.UserSettings", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.BaseUser", "SettingsId", "dbo.UserSettings");
            DropIndex("dbo.BaseUser", new[] { "SettingsId" });
            DropColumn("dbo.BaseUser", "SettingsId");
            DropTable("dbo.UserSettings");
        }
    }
}
