namespace WebApplication14.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _20082016155138 : DbMigration
    {
        public override void Up()
        {
            DropPrimaryKey("dbo.ComplitionScores", new[] { "scoreValue" });
            DropColumn("dbo.ComplitionScores", "scoreValue");
            AddColumn("dbo.ComplitionScores", "ComplitionScoreId", c => c.Int(nullable: false, identity: true));
            AddPrimaryKey("dbo.ComplitionScores", "ComplitionScoreId");
        }
        
            public override void Down()
        {
            DropPrimaryKey("dbo.ComplitionScores", new[] { "scoreValue" });
            DropColumn("dbo.ComplitionScores", "scoreValue");
            AddColumn("dbo.ComplitionScores", "ComplitionScoreId", c => c.Int(nullable: false, identity: true));
            AddPrimaryKey("dbo.ComplitionScores", "ComplitionScoreId");
        }
    }
}
