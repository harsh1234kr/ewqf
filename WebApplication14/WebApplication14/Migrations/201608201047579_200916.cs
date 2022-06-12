namespace WebApplication14.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _200916 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Requirements", "craetedDateTime", c => c.DateTime(nullable: false));
            AddColumn("dbo.Requirements", "isActive", c => c.Boolean(nullable: false));
            AddColumn("dbo.Requirements", "endDatetime", c => c.DateTime(nullable: false));
            AddColumn("dbo.Requirements", "thresholdScore", c => c.Int(nullable: false));
            AddColumn("dbo.Requirements", "isAprroved", c => c.Boolean(nullable: false));
            AlterColumn("dbo.Requirements", "discription", c => c.String());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Requirements", "discription", c => c.String(nullable: false));
            DropColumn("dbo.Requirements", "isAprroved");
            DropColumn("dbo.Requirements", "thresholdScore");
            DropColumn("dbo.Requirements", "endDatetime");
            DropColumn("dbo.Requirements", "isActive");
            DropColumn("dbo.Requirements", "craetedDateTime");
        }
    }
}
