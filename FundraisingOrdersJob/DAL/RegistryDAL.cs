using FundraisingOrdersJob.Models;
using Log4NetLibrary;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace FundraisingOrdersJob.DAL
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
            using (var context = new FundraisingJobContext(_connectionString))
            {
                return context.Configurations.Where(c => c.ParameterName == parameterName && c.ClientId == clientId).SingleOrDefault();
            }
        }

        public List<FundraisingOrdersModel> GetFundraisingOrders(int clientId)
        {
            Logger.Info($"Get Fundraising Orders Data, ClientId: {clientId}, Date: {DateTime.Now:yyyyMMdd}");
            var fundraisingOrdersModel = new List<FundraisingOrdersModel>();
            try
            {
                using (SqlConnection con = new SqlConnection(_connectionString))
                {
                    SqlCommand cmd = new SqlCommand("Report_GetRegistryDailyPhysicalOrders", con);
                    cmd.Parameters.Add(new SqlParameter("ClientID", clientId));
                    cmd.CommandType = CommandType.StoredProcedure;
                    con.Open();
                    SqlDataReader rdr = cmd.ExecuteReader();
                    while (rdr.Read())
                    {
                        fundraisingOrdersModel.Add(new FundraisingOrdersModel
                        {
                            OrderNumber = Convert.ToInt32(rdr[0]),
                            OrderDate = Convert.ToDateTime(rdr[1]),
                            RecipientName = rdr[2].ToString(),
                            Company = rdr[3].ToString(),
                            Email = rdr[4].ToString(),
                            Phone = rdr[5].ToString(),
                            StreetLine1 = rdr[6].ToString(),
                            StreetNumber = rdr[7].ToString(),
                            StreetLine2 = rdr[8].ToString(),
                            City = rdr[9].ToString(),
                            State = rdr[10].ToString(),
                            ZipCode = rdr[11].ToString(),
                            Country = rdr[12].ToString(),
                            ItemTitle = rdr[13].ToString(),
                            SKU = rdr[14].ToString(),
                            Quantity = Convert.IsDBNull(rdr[15]) ? (int?)null : Convert.ToInt32(rdr[15]),
                            ItemWeight = rdr[16].ToString(),
                            ItemPrice = Convert.ToDecimal(rdr[17]),
                            ItemCurrency = rdr[18].ToString(),
                            ShippingMethod = rdr[19].ToString()
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return fundraisingOrdersModel;
        }
    }
}
