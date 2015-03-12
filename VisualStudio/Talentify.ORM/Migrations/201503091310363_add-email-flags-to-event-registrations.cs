namespace Talentify.ORM.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addemailflagstoeventregistrations : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.EventRegistration", "WasNotified", c => c.Boolean(nullable: false));
            AddColumn("dbo.EventRegistration", "HasFollowUpEmail", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.EventRegistration", "HasFollowUpEmail");
            DropColumn("dbo.EventRegistration", "WasNotified");
        }
    }
}
