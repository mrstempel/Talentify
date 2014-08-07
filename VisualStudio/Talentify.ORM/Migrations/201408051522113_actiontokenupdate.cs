namespace Talentify.ORM.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class actiontokenupdate : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ActionToken", "CreatedDate", c => c.DateTime(nullable: false, precision: 7, storeType: "datetime2"));
            AlterColumn("dbo.ActionToken", "ValidUntil", c => c.DateTime(nullable: false, precision: 7, storeType: "datetime2"));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.ActionToken", "ValidUntil", c => c.DateTime(nullable: false));
            DropColumn("dbo.ActionToken", "CreatedDate");
        }
    }
}
