namespace Talentify.ORM.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addausweisguid : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.BaseUser", "AusweisGuid", c => c.Guid());
        }
        
        public override void Down()
        {
            DropColumn("dbo.BaseUser", "AusweisGuid");
        }
    }
}
