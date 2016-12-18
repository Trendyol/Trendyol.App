using System.Data.Entity;
using WebApplication.Models;

namespace WebApplication.Data
{
    public class DataContext : DbContext
    {
        public DbSet<Sample> Samples { get; set; }
    }
}