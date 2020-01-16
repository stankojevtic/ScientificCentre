using Microsoft.EntityFrameworkCore;
using NaucnaCentralaBackend.Models.Database;

namespace NaucnaCentralaBackend.DataContextNamespace
{
    public class DataContext : DbContext
    {
        public DbSet<Magazine> Magazines { get; set; }
        public DbSet<User> Users { get; set; }
        public DataContext(DbContextOptions<DataContext> dbContextOptions) : base(dbContextOptions)
        {
        }
    }
}
