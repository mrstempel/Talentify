namespace Talentify.ORM.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addblockedreason : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.BaseUser", "BlockedReason", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.BaseUser", "BlockedReason");
        }
    }
}
