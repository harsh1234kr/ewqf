namespace WebApplication14.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _260820161648 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Requirements", "thresholdScoreValue", c => c.Double(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Requirements", "thresholdScoreValue");
        }
    }
}
