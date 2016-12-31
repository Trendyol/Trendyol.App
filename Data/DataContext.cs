using System.Data.Entity;
using Domain.Objects;
using Trendyol.App.EntityFramework;

namespace Data
{
    public class DataContext : DataContextBase<DataContext>
    {
        public DataContext()
            : this("DataContext")
        {
        }

        public DataContext(string connectionStringName) 
            : base(connectionStringName)
        {
        }

        public DbSet<Sample> Samples { get; set; }
    }
}
