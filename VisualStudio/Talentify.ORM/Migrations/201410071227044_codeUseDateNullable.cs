namespace Talentify.ORM.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class codeUseDateNullable : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.RegisterCode", "UsedDate", c => c.DateTime(precision: 7, storeType: "datetime2"));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.RegisterCode", "UsedDate", c => c.DateTime(nullable: false, precision: 7, storeType: "datetime2"));
        }
    }
}
