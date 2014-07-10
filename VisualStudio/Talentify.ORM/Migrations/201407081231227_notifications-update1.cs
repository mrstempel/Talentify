namespace Talentify.ORM.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class notificationsupdate1 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Notification", "Text", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Notification", "Text");
        }
    }
}
