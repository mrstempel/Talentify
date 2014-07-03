namespace Talentify.ORM.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class registercode_nullify : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.BaseUser", "RegisterCode", c => c.Guid());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.BaseUser", "RegisterCode", c => c.Guid(nullable: false));
        }
    }
}
