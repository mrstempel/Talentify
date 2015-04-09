namespace Talentify.ORM.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class changetalentecheksex : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.TalentecheckSession", "Frage10", c => c.Int(nullable: false));
            DropColumn("dbo.TalentecheckSession", "Sex");
        }
        
        public override void Down()
        {
            AddColumn("dbo.TalentecheckSession", "Sex", c => c.String());
            DropColumn("dbo.TalentecheckSession", "Frage10");
        }
    }
}
