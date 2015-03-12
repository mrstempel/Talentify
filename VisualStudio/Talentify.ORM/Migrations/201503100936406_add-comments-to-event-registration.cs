namespace Talentify.ORM.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addcommentstoeventregistration : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.EventRegistration", "Comments", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.EventRegistration", "Comments");
        }
    }
}
