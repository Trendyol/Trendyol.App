using System.Data.Entity;
using Domain.Objects;
using Trendyol.App.EntityFramework;

namespace Data
{
    public class SampleDataContext : DataContextBase<SampleDataContext>
    {
        public SampleDataContext()
            : this("SampleDataContext")
        {
        }

        public SampleDataContext(string connectionStringName) 
            : base(connectionStringName, true)
        {
        }

        public DbSet<Sample> Samples { get; set; }
    }
}
