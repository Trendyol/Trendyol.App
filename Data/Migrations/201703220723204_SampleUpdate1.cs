namespace Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class SampleUpdate1 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Samples", "CreatedBy", c => c.String());
            AddColumn("dbo.Samples", "UpdatedOn", c => c.DateTime());
            AddColumn("dbo.Samples", "UpdatedBy", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Samples", "UpdatedBy");
            DropColumn("dbo.Samples", "UpdatedOn");
            DropColumn("dbo.Samples", "CreatedBy");
        }
    }
}
