namespace WebApplication14.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _010920161304 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.MemberScores", "projectId_projectId", "dbo.projects");
            DropIndex("dbo.MemberScores", new[] { "projectId_projectId" });
            AddColumn("dbo.MemberScores", "reqId_reqId", c => c.Int());
            CreateIndex("dbo.MemberScores", "reqId_reqId");
            AddForeignKey("dbo.MemberScores", "reqId_reqId", "dbo.Requirements", "reqId");
            DropColumn("dbo.MemberScores", "projectId_projectId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.MemberScores", "projectId_projectId", c => c.Int());
            DropForeignKey("dbo.MemberScores", "reqId_reqId", "dbo.Requirements");
            DropIndex("dbo.MemberScores", new[] { "reqId_reqId" });
            DropColumn("dbo.MemberScores", "reqId_reqId");
            CreateIndex("dbo.MemberScores", "projectId_projectId");
            AddForeignKey("dbo.MemberScores", "projectId_projectId", "dbo.projects", "projectId");
        }
    }
}
