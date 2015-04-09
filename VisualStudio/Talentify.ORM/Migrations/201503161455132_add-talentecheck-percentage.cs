namespace Talentify.ORM.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addtalentecheckpercentage : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.TalentecheckSession", "MaxPercent", c => c.Int(nullable: false));
            AddColumn("dbo.TalentecheckSession", "MinPercent", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.TalentecheckSession", "MinPercent");
            DropColumn("dbo.TalentecheckSession", "MaxPercent");
        }
    }
}
