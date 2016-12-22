using System.Data.Entity;
using Trendyol.App.EntityFramework;
using WebApplication.Models;

namespace WebApplication.Data
{
    public class Context : DataContextBase
    {
        public Context()
            : base("Context")
        {
        }

        public Context(string connectionStringName)
            : base(connectionStringName)
        {
        }

        public DbSet<Sample> Samples { get; set; }
    }
}