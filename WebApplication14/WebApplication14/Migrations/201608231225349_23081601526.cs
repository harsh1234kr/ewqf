namespace WebApplication14.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _23081601526 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Comments", new[] { "MemberId_MemberId", "MemberId_userName" }, "dbo.Members");
            DropForeignKey("dbo.projects", new[] { "MemberId_MemberId", "MemberId_userName" }, "dbo.Members");
            DropForeignKey("dbo.ComplitionScores", new[] { "MemberId_MemberId", "MemberId_userName" }, "dbo.Members");
            DropForeignKey("dbo.MemberScores", new[] { "MemberId_MemberId", "MemberId_userName" }, "dbo.Members");
            DropForeignKey("dbo.projectMembers", new[] { "MemberId_MemberId", "MemberId_userName" }, "dbo.Members");
            DropIndex("dbo.Comments", new[] { "MemberId_MemberId", "MemberId_userName" });
            DropIndex("dbo.projects", new[] { "MemberId_MemberId", "MemberId_userName" });
            DropIndex("dbo.ComplitionScores", new[] { "MemberId_MemberId", "MemberId_userName" });
            DropIndex("dbo.MemberScores", new[] { "MemberId_MemberId", "MemberId_userName" });
            DropIndex("dbo.projectMembers", new[] { "MemberId_MemberId", "MemberId_userName" });
            DropPrimaryKey("dbo.Members");
            AddPrimaryKey("dbo.Members", "MemberId");
            CreateIndex("dbo.Comments", "MemberId_MemberId");
            CreateIndex("dbo.projects", "MemberId_MemberId");
            CreateIndex("dbo.ComplitionScores", "MemberId_MemberId");
            CreateIndex("dbo.MemberScores", "MemberId_MemberId");
            CreateIndex("dbo.projectMembers", "MemberId_MemberId");
            AddForeignKey("dbo.Comments", "MemberId_MemberId", "dbo.Members", "MemberId");
            AddForeignKey("dbo.projects", "MemberId_MemberId", "dbo.Members", "MemberId");
            AddForeignKey("dbo.ComplitionScores", "MemberId_MemberId", "dbo.Members", "MemberId");
            AddForeignKey("dbo.MemberScores", "MemberId_MemberId", "dbo.Members", "MemberId");
            AddForeignKey("dbo.projectMembers", "MemberId_MemberId", "dbo.Members", "MemberId");
            DropColumn("dbo.Comments", "MemberId_userName");
            DropColumn("dbo.projects", "MemberId_userName");
            DropColumn("dbo.ComplitionScores", "MemberId_userName");
            DropColumn("dbo.MemberScores", "MemberId_userName");
            DropColumn("dbo.projectMembers", "MemberId_userName");
        }
        
        public override void Down()
        {
            AddColumn("dbo.projectMembers", "MemberId_userName", c => c.String(maxLength: 150));
            AddColumn("dbo.MemberScores", "MemberId_userName", c => c.String(maxLength: 150));
            AddColumn("dbo.ComplitionScores", "MemberId_userName", c => c.String(maxLength: 150));
            AddColumn("dbo.projects", "MemberId_userName", c => c.String(maxLength: 150));
            AddColumn("dbo.Comments", "MemberId_userName", c => c.String(maxLength: 150));
            DropForeignKey("dbo.projectMembers", "MemberId_MemberId", "dbo.Members");
            DropForeignKey("dbo.MemberScores", "MemberId_MemberId", "dbo.Members");
            DropForeignKey("dbo.ComplitionScores", "MemberId_MemberId", "dbo.Members");
            DropForeignKey("dbo.projects", "MemberId_MemberId", "dbo.Members");
            DropForeignKey("dbo.Comments", "MemberId_MemberId", "dbo.Members");
            DropIndex("dbo.projectMembers", new[] { "MemberId_MemberId" });
            DropIndex("dbo.MemberScores", new[] { "MemberId_MemberId" });
            DropIndex("dbo.ComplitionScores", new[] { "MemberId_MemberId" });
            DropIndex("dbo.projects", new[] { "MemberId_MemberId" });
            DropIndex("dbo.Comments", new[] { "MemberId_MemberId" });
            DropPrimaryKey("dbo.Members");
            AddPrimaryKey("dbo.Members", new[] { "MemberId", "userName" });
            CreateIndex("dbo.projectMembers", new[] { "MemberId_MemberId", "MemberId_userName" });
            CreateIndex("dbo.MemberScores", new[] { "MemberId_MemberId", "MemberId_userName" });
            CreateIndex("dbo.ComplitionScores", new[] { "MemberId_MemberId", "MemberId_userName" });
            CreateIndex("dbo.projects", new[] { "MemberId_MemberId", "MemberId_userName" });
            CreateIndex("dbo.Comments", new[] { "MemberId_MemberId", "MemberId_userName" });
            AddForeignKey("dbo.projectMembers", new[] { "MemberId_MemberId", "MemberId_userName" }, "dbo.Members", new[] { "MemberId", "userName" });
            AddForeignKey("dbo.MemberScores", new[] { "MemberId_MemberId", "MemberId_userName" }, "dbo.Members", new[] { "MemberId", "userName" });
            AddForeignKey("dbo.ComplitionScores", new[] { "MemberId_MemberId", "MemberId_userName" }, "dbo.Members", new[] { "MemberId", "userName" });
            AddForeignKey("dbo.projects", new[] { "MemberId_MemberId", "MemberId_userName" }, "dbo.Members", new[] { "MemberId", "userName" });
            AddForeignKey("dbo.Comments", new[] { "MemberId_MemberId", "MemberId_userName" }, "dbo.Members", new[] { "MemberId", "userName" });
        }
    }
}
