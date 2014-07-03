namespace Talentify.ORM.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class usernewprofilefields : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Student", "AboutMe", c => c.String());
            AddColumn("dbo.Student", "IsCoachingEnabled", c => c.Boolean(nullable: false));
            AddColumn("dbo.Student", "CoachingPrice", c => c.Decimal(nullable: false, precision: 18, scale: 2));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Student", "CoachingPrice");
            DropColumn("dbo.Student", "IsCoachingEnabled");
            DropColumn("dbo.Student", "AboutMe");
        }
    }
}
