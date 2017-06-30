namespace Data.Migrations
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity.Infrastructure.Annotations;
    using System.Data.Entity.Migrations;
    
    public partial class Filter : DbMigration
    {
        public override void Up()
        {
            AlterTableAnnotations(
                "dbo.Samples",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Name = c.String(),
                        Size = c.Int(nullable: false),
                        CreatedOn = c.DateTime(nullable: false),
                        CreatedBy = c.String(),
                        UpdatedOn = c.DateTime(),
                        UpdatedBy = c.String(),
                        IsDeleted = c.Boolean(nullable: false),
                        TestField = c.String(),
                        TestLongFieldWithLongName = c.Long(nullable: false),
                    },
                annotations: new Dictionary<string, AnnotationValues>
                {
                    { 
                        "DynamicFilter_Sample_SoftDeleteFilter",
                        new AnnotationValues(oldValue: null, newValue: "EntityFramework.DynamicFilters.DynamicFilterDefinition")
                    },
                    { 
                        "globalFilter_SoftDeleteFilter",
                        new AnnotationValues(oldValue: "EntityFramework.Filters.FilterDefinition", newValue: null)
                    },
                });
            
        }
        
        public override void Down()
        {
            AlterTableAnnotations(
                "dbo.Samples",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Name = c.String(),
                        Size = c.Int(nullable: false),
                        CreatedOn = c.DateTime(nullable: false),
                        CreatedBy = c.String(),
                        UpdatedOn = c.DateTime(),
                        UpdatedBy = c.String(),
                        IsDeleted = c.Boolean(nullable: false),
                        TestField = c.String(),
                        TestLongFieldWithLongName = c.Long(nullable: false),
                    },
                annotations: new Dictionary<string, AnnotationValues>
                {
                    { 
                        "DynamicFilter_Sample_SoftDeleteFilter",
                        new AnnotationValues(oldValue: "EntityFramework.DynamicFilters.DynamicFilterDefinition", newValue: null)
                    },
                    { 
                        "globalFilter_SoftDeleteFilter",
                        new AnnotationValues(oldValue: null, newValue: "EntityFramework.Filters.FilterDefinition")
                    },
                });
            
        }
    }
}
