using RegistryJob.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;

namespace RegistryJob.DAL
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
