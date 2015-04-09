namespace Talentify.ORM.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addinvitesessionid : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.TalentecheckSession", "InviteSessionId", c => c.Guid(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.TalentecheckSession", "InviteSessionId");
        }
    }
}
