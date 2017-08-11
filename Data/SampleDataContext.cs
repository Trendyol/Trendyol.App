using System.Data.Entity;
using Domain.Objects;
using Trendyol.App.EntityFramework;

namespace Data
{
    public class SampleDataContext : DataContextBase
    {
        public SampleDataContext()
            : this("SampleDataContext")
        {
        }

        public SampleDataContext(string connectionStringName) 
            : base(connectionStringName, false)
        {
        }

        public DbSet<Sample> Samples { get; set; }
    }
}
