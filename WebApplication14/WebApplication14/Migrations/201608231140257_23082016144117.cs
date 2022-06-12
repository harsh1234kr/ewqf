namespace WebApplication14.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _23082016144117 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Comments", "MemberId_MemberId", "dbo.Members");
            DropForeignKey("dbo.projects", "MemberId_MemberId", "dbo.Members");
            DropForeignKey("dbo.ComplitionScores", "MemberId_MemberId", "dbo.Members");
            DropForeignKey("dbo.MemberScores", "MemberId_MemberId", "dbo.Members");
            DropForeignKey("dbo.projectMembers", "MemberId_MemberId", "dbo.Members");
            DropIndex("dbo.Comments", new[] { "MemberId_MemberId" });
            DropIndex("dbo.projects", new[] { "MemberId_MemberId" });
            DropIndex("dbo.ComplitionScores", new[] { "MemberId_MemberId" });
            DropIndex("dbo.MemberScores", new[] { "MemberId_MemberId" });
            DropIndex("dbo.projectMembers", new[] { "MemberId_MemberId" });
            DropPrimaryKey("dbo.Members");
            AddColumn("dbo.Comments", "MemberId_userName", c => c.String(maxLength: 150));
            AddColumn("dbo.projects", "MemberId_userName", c => c.String(maxLength: 150));
            AddColumn("dbo.ComplitionScores", "MemberId_userName", c => c.String(maxLength: 150));
            AddColumn("dbo.MemberScores", "MemberId_userName", c => c.String(maxLength: 150));
            AddColumn("dbo.projectMembers", "MemberId_userName", c => c.String(maxLength: 150));
            AlterColumn("dbo.Members", "firstName", c => c.String(nullable: false, maxLength: 150));
            AlterColumn("dbo.Members", "lastName", c => c.String(nullable: false, maxLength: 150));
            AlterColumn("dbo.Members", "userName", c => c.String(nullable: false, maxLength: 150));
            AlterColumn("dbo.Members", "phone", c => c.String(maxLength: 12));
            AlterColumn("dbo.Members", "password", c => c.String(nullable: false, maxLength: 30));
            AlterColumn("dbo.projects", "projectName", c => c.String(nullable: false, maxLength: 150));
            AlterColumn("dbo.projects", "discription", c => c.String(nullable: false, maxLength: 3000));
            AlterColumn("dbo.Requirements", "title", c => c.String(nullable: false, maxLength: 150));
            AlterColumn("dbo.Requirements", "discription", c => c.String(nullable: false, maxLength: 3000));
            AddPrimaryKey("dbo.Members", new[] { "MemberId", "userName" });
            CreateIndex("dbo.Comments", new[] { "MemberId_MemberId", "MemberId_userName" });
            CreateIndex("dbo.projects", new[] { "MemberId_MemberId", "MemberId_userName" });
            CreateIndex("dbo.ComplitionScores", new[] { "MemberId_MemberId", "MemberId_userName" });
            CreateIndex("dbo.MemberScores", new[] { "MemberId_MemberId", "MemberId_userName" });
            CreateIndex("dbo.projectMembers", new[] { "MemberId_MemberId", "MemberId_userName" });
            AddForeignKey("dbo.Comments", new[] { "MemberId_MemberId", "MemberId_userName" }, "dbo.Members", new[] { "MemberId", "userName" });
            AddForeignKey("dbo.projects", new[] { "MemberId_MemberId", "MemberId_userName" }, "dbo.Members", new[] { "MemberId", "userName" });
            AddForeignKey("dbo.ComplitionScores", new[] { "MemberId_MemberId", "MemberId_userName" }, "dbo.Members", new[] { "MemberId", "userName" });
            AddForeignKey("dbo.MemberScores", new[] { "MemberId_MemberId", "MemberId_userName" }, "dbo.Members", new[] { "MemberId", "userName" });
            AddForeignKey("dbo.projectMembers", new[] { "MemberId_MemberId", "MemberId_userName" }, "dbo.Members", new[] { "MemberId", "userName" });
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.projectMembers", new[] { "MemberId_MemberId", "MemberId_userName" }, "dbo.Members");
            DropForeignKey("dbo.MemberScores", new[] { "MemberId_MemberId", "MemberId_userName" }, "dbo.Members");
            DropForeignKey("dbo.ComplitionScores", new[] { "MemberId_MemberId", "MemberId_userName" }, "dbo.Members");
            DropForeignKey("dbo.projects", new[] { "MemberId_MemberId", "MemberId_userName" }, "dbo.Members");
            DropForeignKey("dbo.Comments", new[] { "MemberId_MemberId", "MemberId_userName" }, "dbo.Members");
            DropIndex("dbo.projectMembers", new[] { "MemberId_MemberId", "MemberId_userName" });
            DropIndex("dbo.MemberScores", new[] { "MemberId_MemberId", "MemberId_userName" });
            DropIndex("dbo.ComplitionScores", new[] { "MemberId_MemberId", "MemberId_userName" });
            DropIndex("dbo.projects", new[] { "MemberId_MemberId", "MemberId_userName" });
            DropIndex("dbo.Comments", new[] { "MemberId_MemberId", "MemberId_userName" });
            DropPrimaryKey("dbo.Members");
            AlterColumn("dbo.Requirements", "discription", c => c.String(nullable: false));
            AlterColumn("dbo.Requirements", "title", c => c.String(nullable: false));
            AlterColumn("dbo.projects", "discription", c => c.String(nullable: false));
            AlterColumn("dbo.projects", "projectName", c => c.String(nullable: false));
            AlterColumn("dbo.Members", "password", c => c.String(nullable: false));
            AlterColumn("dbo.Members", "phone", c => c.Int(nullable: false));
            AlterColumn("dbo.Members", "userName", c => c.String(nullable: false));
            AlterColumn("dbo.Members", "lastName", c => c.String(nullable: false));
            AlterColumn("dbo.Members", "firstName", c => c.String(nullable: false));
            DropColumn("dbo.projectMembers", "MemberId_userName");
            DropColumn("dbo.MemberScores", "MemberId_userName");
            DropColumn("dbo.ComplitionScores", "MemberId_userName");
            DropColumn("dbo.projects", "MemberId_userName");
            DropColumn("dbo.Comments", "MemberId_userName");
            AddPrimaryKey("dbo.Members", "MemberId");
            CreateIndex("dbo.projectMembers", "MemberId_MemberId");
            CreateIndex("dbo.MemberScores", "MemberId_MemberId");
            CreateIndex("dbo.ComplitionScores", "MemberId_MemberId");
            CreateIndex("dbo.projects", "MemberId_MemberId");
            CreateIndex("dbo.Comments", "MemberId_MemberId");
            AddForeignKey("dbo.projectMembers", "MemberId_MemberId", "dbo.Members", "MemberId");
            AddForeignKey("dbo.MemberScores", "MemberId_MemberId", "dbo.Members", "MemberId");
            AddForeignKey("dbo.ComplitionScores", "MemberId_MemberId", "dbo.Members", "MemberId");
            AddForeignKey("dbo.projects", "MemberId_MemberId", "dbo.Members", "MemberId");
            AddForeignKey("dbo.Comments", "MemberId_MemberId", "dbo.Members", "MemberId");
        }
    }
}
