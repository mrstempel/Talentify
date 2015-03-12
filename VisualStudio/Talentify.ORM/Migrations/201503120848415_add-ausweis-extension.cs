namespace Talentify.ORM.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addausweisextension : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.BaseUser", "AusweisExtension", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.BaseUser", "AusweisExtension");
        }
    }
}
