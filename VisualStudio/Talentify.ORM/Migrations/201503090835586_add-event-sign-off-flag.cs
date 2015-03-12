namespace Talentify.ORM.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addeventsignoffflag : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.EventRegistration", "IsSignedOff", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.EventRegistration", "IsSignedOff");
        }
    }
}
