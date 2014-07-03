namespace Talentify.ORM.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class userprofilepicture : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.BaseUser", "PictureGuid", c => c.Guid());
            AddColumn("dbo.BaseUser", "IsPictureLandscape", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.BaseUser", "IsPictureLandscape");
            DropColumn("dbo.BaseUser", "PictureGuid");
        }
    }
}
