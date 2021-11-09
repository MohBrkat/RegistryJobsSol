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
            _connectionString = iconfiguration.GetConnectionString("Default");
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
            var registryDetailsDataModel = new List<DailyReigstryDetailsModel>();
            try
            {
                using (SqlConnection con = new SqlConnection(_connectionString))
                {
                    SqlCommand cmd = new SqlCommand("Report_DailyRegistryDetails", con);
                    cmd.Parameters.Add(new SqlParameter("ClientID", clientId));
                    cmd.CommandType = CommandType.StoredProcedure;
                    con.Open();
                    SqlDataReader rdr = cmd.ExecuteReader();
                    while (rdr.Read())
                    {
                        registryDetailsDataModel.Add(new DailyReigstryDetailsModel
                        {
                            ListNumher = Convert.ToInt32(rdr[0]),
                            RegistrantFirstName = GetEncoded(rdr[1].ToString()?.Trim()),
                            RegistrantLastName = GetEncoded(rdr[2].ToString()?.Trim()),
                            CoRegistrantFirstName = GetEncoded(rdr[3].ToString()?.Trim()),
                            CoRegistrantLastName = GetEncoded(rdr[4].ToString()?.Trim()),
                            RegistrantPhone = rdr[5].ToString()?.Trim(),
                            RegistrantEmail = GetEncoded(rdr[6].ToString()?.Trim()),
                            CoRegistrantEmail = (rdr[7].ToString()?.Contains("@")) != true ? string.Empty : rdr[7].ToString()?.Trim(),
                            CoRegistrantPhone = rdr[8].ToString()?.Trim(),
                            Country = GetEncoded(rdr[9].ToString()?.Trim()),
                            City = GetEncoded(rdr[10].ToString()?.Trim()),
                            AddressLine1 = GetEncoded(rdr[11].ToString()?.Trim()),
                            AddressLine2 = GetEncoded(rdr[12].ToString()?.Trim()),
                            Zip = rdr[13].ToString()?.Trim(),
                            Province = GetEncoded(rdr[14].ToString()?.Trim()),
                            County = GetEncoded(rdr[15].ToString()?.Trim()),
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return registryDetailsDataModel;
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
