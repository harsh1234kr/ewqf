namespace WebApplication14.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _210820162213 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Members", "pic", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Members", "pic");
        }
    }
}
