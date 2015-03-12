namespace Talentify.ORM.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addworkshopblockedflag : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.BaseUser", "IsWorkshopBlocked", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.BaseUser", "IsWorkshopBlocked");
        }
    }
}
