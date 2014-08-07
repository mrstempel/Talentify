namespace Talentify.ORM.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class additionalnotificationinfo : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Notification", "AdditionalInfo", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Notification", "AdditionalInfo");
        }
    }
}
