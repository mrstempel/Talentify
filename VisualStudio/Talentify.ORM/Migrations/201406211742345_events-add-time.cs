namespace Talentify.ORM.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class eventsaddtime : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Event", "BeginTime", c => c.String());
            AddColumn("dbo.Event", "EndTime", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Event", "EndTime");
            DropColumn("dbo.Event", "BeginTime");
        }
    }
}
