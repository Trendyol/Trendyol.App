namespace WebApplication.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class SampleUpdate : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Samples", "Size", c => c.Int(nullable: false));
            AddColumn("dbo.Samples", "CreatedOn", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Samples", "CreatedOn");
            DropColumn("dbo.Samples", "Size");
        }
    }
}
