using RegistryJob.Models;
using System;
using System.Data.Entity;
using System.Data.Entity.Core.Objects;
using System.Data.Entity.Infrastructure;
using System.Data.SqlClient;

namespace RegistryJob.DAL
{
    public class DatabaseContext : DbContext
    {
        public virtual DbSet<Configuration> Configurations { get; set; }

        public DatabaseContext(string _connectionString)
            : base(_connectionString)
        {
        }

        //private static DbContextOptions GetOptions(string connectionString)
        //{
        //    return SqlServerDbContextOptionsExtensions.UseSqlServer(new DbContextOptionsBuilder(), connectionString).Options;
        //}

        public virtual ObjectResult<DailyReigstryDetailsModel> Report_DailyRegistryDetails(Nullable<int> clientId)
        {
            var clientIdParameter = clientId.HasValue ?
                new SqlParameter("ClientId", clientId) :
                new SqlParameter("ClientId", typeof(int));

            return ((IObjectContextAdapter)this).ObjectContext.ExecuteStoreQuery<DailyReigstryDetailsModel>("dbo.Report_DailyRegistryDetails @ClientId", clientIdParameter);
        }
    }
}
