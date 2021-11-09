using RegistryJob.Models;
using Log4NetLibrary;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

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

        public List<DailyReigstryDetailsModel> GetDailyRegistryDetailsData(int clientId)
        {
            Logger.Info($"Get Daily Registry Details Data, ClientId: {clientId}, Date: {DateTime.Now:yyyyMMdd}");
            try
            {
                using (var context = new DatabaseContext(_connectionString))
                {
                    return context.Report_DailyRegistryDetails(clientId).ToList();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public string GetEncoded(string dbString)
        {
            Encoding iso = Encoding.GetEncoding("ISO-8859-1");
            Encoding utf8 = Encoding.UTF8;
            byte[] utfBytes = utf8.GetBytes(dbString);
            byte[] isoBytes = Encoding.Convert(utf8, iso, utfBytes);
            string msg = iso.GetString(isoBytes);
            return msg;
        }
    }
}
