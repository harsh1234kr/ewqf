namespace WebApplication14.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _010920161409 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Members", "scoreMember", c => c.Int(nullable: false));
            AddColumn("dbo.Members", "grade", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Members", "grade");
            DropColumn("dbo.Members", "scoreMember");
        }
    }
}
