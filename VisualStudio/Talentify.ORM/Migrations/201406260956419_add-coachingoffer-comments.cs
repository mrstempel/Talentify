namespace Talentify.ORM.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addcoachingoffercomments : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.CoachingOffer", "Comments", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.CoachingOffer", "Comments");
        }
    }
}
