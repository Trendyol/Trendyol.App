using System;
using Trendyol.App.Data;

namespace Domain.Objects
{
    public class Sample : IEntity<long>, IAuditable, ISoftDeletable
    {
        public long Id { get; set; }

        public string Name { get; set; }

        public int Size { get; set; }

        public DateTime CreatedOn { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? UpdatedOn { get; set; }
        public string UpdatedBy { get; set; }

        public bool IsDeleted { get; set; }

        public string TestField { get; set; }

        public long TestLongFieldWithLongName { get; set; }
    }
}