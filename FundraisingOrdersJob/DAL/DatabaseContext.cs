using DailyReigstryDetailsJob.Models;
using Microsoft.EntityFrameworkCore;

namespace DailyReigstryDetailsJob.DAL
{
    public class DatabaseContext : DbContext
    {
        public virtual DbSet<Configuration> Configurations { get; set; }

        public DatabaseContext(string _connectionString)
            : base(GetOptions(_connectionString))
        {
        }

        private static DbContextOptions GetOptions(string connectionString)
        {
            return SqlServerDbContextOptionsExtensions.UseSqlServer(new DbContextOptionsBuilder(), connectionString).Options;
        }
    }
}
