namespace WebApplication14.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _010920161125 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.SubRequirments", "SubReqId", "dbo.Requirements");
            DropForeignKey("dbo.SubRequirments", "ParentReqId", "dbo.Requirements");
            DropIndex("dbo.SubRequirments", new[] { "ParentReqId" });
            DropIndex("dbo.SubRequirments", new[] { "SubReqId" });
            DropTable("dbo.chats");
            DropTable("dbo.SubRequirments");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.SubRequirments",
                c => new
                    {
                        ParentReqId = c.Int(nullable: false),
                        SubReqId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.ParentReqId, t.SubReqId });
            
            CreateTable(
                "dbo.chats",
                c => new
                    {
                        chatId = c.Int(nullable: false, identity: true),
                        CreateDateTime = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.chatId);
            
            CreateIndex("dbo.SubRequirments", "SubReqId");
            CreateIndex("dbo.SubRequirments", "ParentReqId");
            AddForeignKey("dbo.SubRequirments", "ParentReqId", "dbo.Requirements", "reqId");
            AddForeignKey("dbo.SubRequirments", "SubReqId", "dbo.Requirements", "reqId");
        }
    }
}
