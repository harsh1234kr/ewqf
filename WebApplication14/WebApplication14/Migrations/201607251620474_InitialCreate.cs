namespace WebApplication14.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.chats",
                c => new
                    {
                        chatId = c.Int(nullable: false, identity: true),
                        CreateDateTime = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.chatId);
            
            CreateTable(
                "dbo.Comments",
                c => new
                    {
                        commentId = c.Int(nullable: false, identity: true),
                        CreateDatetime = c.DateTime(nullable: false),
                        Content = c.String(nullable: false),
                        chatId_chatId = c.Int(),
                        MemberId_MemberId = c.Int(),
                        projectId_projectId = c.Int(),
                    })
                .PrimaryKey(t => t.commentId)
                .ForeignKey("dbo.chats", t => t.chatId_chatId)
                .ForeignKey("dbo.Members", t => t.MemberId_MemberId)
                .ForeignKey("dbo.projects", t => t.projectId_projectId)
                .Index(t => t.chatId_chatId)
                .Index(t => t.MemberId_MemberId)
                .Index(t => t.projectId_projectId);
            
            CreateTable(
                "dbo.Members",
                c => new
                    {
                        MemberId = c.Int(nullable: false, identity: true),
                        firstName = c.String(nullable: false),
                        lastName = c.String(nullable: false),
                        userName = c.String(nullable: false),
                        email = c.String(nullable: false),
                        phone = c.Int(nullable: false),
                        password = c.String(nullable: false),
                        IsAdmin = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.MemberId);
            
            CreateTable(
                "dbo.projects",
                c => new
                    {
                        projectId = c.Int(nullable: false, identity: true),
                        projectName = c.String(nullable: false),
                        discription = c.String(nullable: false),
                        MemberId_MemberId = c.Int(),
                    })
                .PrimaryKey(t => t.projectId)
                .ForeignKey("dbo.Members", t => t.MemberId_MemberId)
                .Index(t => t.MemberId_MemberId);
            
            CreateTable(
                "dbo.ComplitionScores",
                c => new
                    {
                        scoreValue = c.Int(nullable: false, identity: true),
                        MemberId_MemberId = c.Int(),
                        projectId_projectId = c.Int(),
                    })
                .PrimaryKey(t => t.scoreValue)
                .ForeignKey("dbo.Members", t => t.MemberId_MemberId)
                .ForeignKey("dbo.projects", t => t.projectId_projectId)
                .Index(t => t.MemberId_MemberId)
                .Index(t => t.projectId_projectId);
            
            CreateTable(
                "dbo.MemberScores",
                c => new
                    {
                        memberScoreId = c.Int(nullable: false, identity: true),
                        createDateTime = c.DateTime(nullable: false),
                        ScoreValue = c.Int(nullable: false),
                        description = c.String(),
                        MemberId_MemberId = c.Int(),
                        projectId_projectId = c.Int(),
                    })
                .PrimaryKey(t => t.memberScoreId)
                .ForeignKey("dbo.Members", t => t.MemberId_MemberId)
                .ForeignKey("dbo.projects", t => t.projectId_projectId)
                .Index(t => t.MemberId_MemberId)
                .Index(t => t.projectId_projectId);
            
            CreateTable(
                "dbo.projectMembers",
                c => new
                    {
                        joinDate = c.DateTime(nullable: false),
                        MemberId_MemberId = c.Int(),
                        projectId_projectId = c.Int(),
                        roleId_roleId = c.Int(),
                    })
                .PrimaryKey(t => t.joinDate)
                .ForeignKey("dbo.Members", t => t.MemberId_MemberId)
                .ForeignKey("dbo.projects", t => t.projectId_projectId)
                .ForeignKey("dbo.Roles", t => t.roleId_roleId)
                .Index(t => t.MemberId_MemberId)
                .Index(t => t.projectId_projectId)
                .Index(t => t.roleId_roleId);
            
            CreateTable(
                "dbo.Roles",
                c => new
                    {
                        roleId = c.Int(nullable: false, identity: true),
                        roleName = c.String(nullable: false),
                        roleDescription = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.roleId);
            
            CreateTable(
                "dbo.Requirements",
                c => new
                    {
                        reqId = c.Int(nullable: false, identity: true),
                        title = c.String(nullable: false),
                        discription = c.String(nullable: false),
                        Total_Complition_Score = c.Int(nullable: false),
                        projectId_projectId = c.Int(),
                        typeId_typeId = c.Int(),
                    })
                .PrimaryKey(t => t.reqId)
                .ForeignKey("dbo.projects", t => t.projectId_projectId)
                .ForeignKey("dbo.RequirementTypes", t => t.typeId_typeId)
                .Index(t => t.projectId_projectId)
                .Index(t => t.typeId_typeId);
            
            CreateTable(
                "dbo.RequirementTypes",
                c => new
                    {
                        typeId = c.Int(nullable: false, identity: true),
                        typeName = c.String(nullable: false),
                        typeDiscription = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.typeId);
            
            CreateTable(
                "dbo.SubRequirments",
                c => new
                    {
                        ParentReqId = c.Int(nullable: false),
                        SubReqId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.ParentReqId, t.SubReqId })
                .ForeignKey("dbo.Requirements", t => t.SubReqId)
                .ForeignKey("dbo.Requirements", t => t.ParentReqId)
                .Index(t => t.ParentReqId)
                .Index(t => t.SubReqId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.SubRequirments", "ParentReqId", "dbo.Requirements");
            DropForeignKey("dbo.SubRequirments", "SubReqId", "dbo.Requirements");
            DropForeignKey("dbo.Requirements", "typeId_typeId", "dbo.RequirementTypes");
            DropForeignKey("dbo.Requirements", "projectId_projectId", "dbo.projects");
            DropForeignKey("dbo.projectMembers", "roleId_roleId", "dbo.Roles");
            DropForeignKey("dbo.projectMembers", "projectId_projectId", "dbo.projects");
            DropForeignKey("dbo.projectMembers", "MemberId_MemberId", "dbo.Members");
            DropForeignKey("dbo.MemberScores", "projectId_projectId", "dbo.projects");
            DropForeignKey("dbo.MemberScores", "MemberId_MemberId", "dbo.Members");
            DropForeignKey("dbo.ComplitionScores", "projectId_projectId", "dbo.projects");
            DropForeignKey("dbo.ComplitionScores", "MemberId_MemberId", "dbo.Members");
            DropForeignKey("dbo.Comments", "projectId_projectId", "dbo.projects");
            DropForeignKey("dbo.projects", "MemberId_MemberId", "dbo.Members");
            DropForeignKey("dbo.Comments", "MemberId_MemberId", "dbo.Members");
            DropForeignKey("dbo.Comments", "chatId_chatId", "dbo.chats");
            DropIndex("dbo.SubRequirments", new[] { "SubReqId" });
            DropIndex("dbo.SubRequirments", new[] { "ParentReqId" });
            DropIndex("dbo.Requirements", new[] { "typeId_typeId" });
            DropIndex("dbo.Requirements", new[] { "projectId_projectId" });
            DropIndex("dbo.projectMembers", new[] { "roleId_roleId" });
            DropIndex("dbo.projectMembers", new[] { "projectId_projectId" });
            DropIndex("dbo.projectMembers", new[] { "MemberId_MemberId" });
            DropIndex("dbo.MemberScores", new[] { "projectId_projectId" });
            DropIndex("dbo.MemberScores", new[] { "MemberId_MemberId" });
            DropIndex("dbo.ComplitionScores", new[] { "projectId_projectId" });
            DropIndex("dbo.ComplitionScores", new[] { "MemberId_MemberId" });
            DropIndex("dbo.projects", new[] { "MemberId_MemberId" });
            DropIndex("dbo.Comments", new[] { "projectId_projectId" });
            DropIndex("dbo.Comments", new[] { "MemberId_MemberId" });
            DropIndex("dbo.Comments", new[] { "chatId_chatId" });
            DropTable("dbo.SubRequirments");
            DropTable("dbo.RequirementTypes");
            DropTable("dbo.Requirements");
            DropTable("dbo.Roles");
            DropTable("dbo.projectMembers");
            DropTable("dbo.MemberScores");
            DropTable("dbo.ComplitionScores");
            DropTable("dbo.projects");
            DropTable("dbo.Members");
            DropTable("dbo.Comments");
            DropTable("dbo.chats");
        }
    }
}
