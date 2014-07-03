namespace Talentify.ORM.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class eventsaddlocationandcountry : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Event", "LocationName", c => c.String());
            AddColumn("dbo.Event", "Country", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Event", "Country");
            DropColumn("dbo.Event", "LocationName");
        }
    }
}
