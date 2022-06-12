namespace WebApplication14.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _200820161605 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ComplitionScores", "scoreValue", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            AddColumn("dbo.ComplitionScores", "scoreValue", c => c.Int(nullable: false));
        }
    }
}
