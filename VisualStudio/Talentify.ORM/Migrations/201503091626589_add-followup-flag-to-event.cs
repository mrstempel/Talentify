namespace Talentify.ORM.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addfollowupflagtoevent : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Event", "HasFollowUpCompleted", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Event", "HasFollowUpCompleted");
        }
    }
}
