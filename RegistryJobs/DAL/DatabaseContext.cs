using RegistryJob.Models;
using System;
using System.Data.Entity;
using System.Data.Entity.Core.Objects;
using System.Data.Entity.Infrastructure;
using System.Data.SqlClient;

namespace RegistryJob.DAL
{
    public partial class DatabaseContext : DbContext
    {
        public virtual DbSet<Configuration> Configurations { get; set; }

        public DatabaseContext(string _connectionString)
            : base(_connectionString)
        {
        }

        //private static Microsoft.EntityFrameworkCore.DbContextOptions GetOptions(string connectionString)
        //{
        //    return Microsoft.EntityFrameworkCore.SqlServerDbContextOptionsExtensions.UseSqlServer(new DbContextOptionsBuilder(), connectionString).Options;
        //}

        public virtual ObjectResult<FundraisingOrdersModel> Report_GetRegistryDailyPhysicalOrders(Nullable<int> clientId)
        {
            var clientIdParameter = clientId.HasValue ?
                new SqlParameter("ClientId", clientId) :
                new SqlParameter("ClientId", typeof(int));

            return ((IObjectContextAdapter)this).ObjectContext.ExecuteStoreQuery<FundraisingOrdersModel>("Report_GetRegistryDailyPhysicalOrders @ClientId", clientIdParameter);
        }
    }
}
