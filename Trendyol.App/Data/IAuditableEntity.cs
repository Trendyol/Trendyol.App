using System;

namespace Trendyol.App.Data
{
    public interface IAuditableEntity<T> : IEntity<T>
    {
        DateTime CreatedOn { get; set; }

        string CreatedBy { get; set; }

        DateTime? UpdatedOn { get; set; }

        string UpdatedBy { get; set; }
    }
}