namespace Talentify.ORM.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addgender : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.BaseUser", "Gender", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.BaseUser", "Gender");
        }
    }
}
