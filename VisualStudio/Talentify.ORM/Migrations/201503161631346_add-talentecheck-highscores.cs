namespace Talentify.ORM.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addtalentecheckhighscores : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.TalentecheckHighscores",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        TalentecheckSessionId = c.Int(nullable: false),
                        Points = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.TalentecheckHighscores");
        }
    }
}
