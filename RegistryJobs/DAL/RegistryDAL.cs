using RegistryJob.Models;
using Log4NetLibrary;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace RegistryJob.DAL
{
    public class RegistryDAL
    {
        private string _connectionString;
        public RegistryDAL(IConfiguration iconfiguration)
        {
            _connectionString = iconfiguration.GetConnectionString("RegistryEntities");
        }

        public Configuration GetConfigurations(string parameterName, int clientId = 0)
        {
            using (var context = new DatabaseContext(_connectionString))
            {
                return context.Configurations.Where(c => c.ParameterName == parameterName && c.ClientId == clientId).SingleOrDefault();
            }
        }

        public List<FundraisingOrdersModel> GetFundraisingOrders(int clientId)
        {
            Logger.Info($"Get Fundraising Orders Data, ClientId: {clientId}, Date: {DateTime.Now:yyyyMMdd}");
            try
            {
                using (var context = new DatabaseContext(_connectionString))
                {
                    return context.Report_GetRegistryDailyPhysicalOrders(clientId).ToList();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
