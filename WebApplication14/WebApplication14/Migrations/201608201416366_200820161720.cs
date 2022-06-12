namespace WebApplication14.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _200820161720 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Requirements", "craetedDateTime", c => c.DateTime(nullable: false, precision: 7, storeType: "datetime2"));
            AlterColumn("dbo.Requirements", "endDatetime", c => c.DateTime(nullable: false, precision: 7, storeType: "datetime2"));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Requirements", "endDatetime", c => c.DateTime(nullable: false));
            AlterColumn("dbo.Requirements", "craetedDateTime", c => c.DateTime(nullable: false));
        }
    }
}
