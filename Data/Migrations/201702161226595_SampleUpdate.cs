namespace Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class SampleUpdate : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Samples", "TestField", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Samples", "TestField");
        }
    }
}
