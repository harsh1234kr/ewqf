namespace WebApplication14.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _210820162128 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Requirements", "discription", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Requirements", "discription", c => c.String());
        }
    }
}
