namespace Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class TestFieldUpdate : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Samples", "TestLongFieldWithLongName", c => c.Long(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Samples", "TestLongFieldWithLongName");
        }
    }
}
