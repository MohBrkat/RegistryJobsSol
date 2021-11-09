using RegistryJob.DAL;
using RegistryJob.Helpers;
using RegistryJob.Models;
using Log4NetLibrary;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace RegistryJob
{
    public class Program
    {
        private static IConfiguration _iconfiguration;
        static void Main(string[] args)
        {
            try
            {
                GetAppSettingsFile();

                var registryDAL = new RegistryDAL(_iconfiguration);
                var clientIdsString = registryDAL.GetConfigurations("FundraisingJobClientIds")?.Value;

                List<string> clientIdList = clientIdsString.Split(',').ToList();
                foreach (var clientId in clientIdList)
                {
                    GenerateDailyFundraisingOrdersReport(clientId);
                }

            }
            catch (Exception ex)
            {
                Logger.Error(ex.Message);
                new SendEmailHelper(_iconfiguration).SendEmail(ex, "Fundraising Job Error: Job Failed.");
            }
        }

        private static void GenerateDailyFundraisingOrdersReport(string clientId)
        {
            string fileName = string.Empty;

            var registryDAL = new RegistryDAL(_iconfiguration);
            string reportName = registryDAL.GetConfigurations("FundraisingJobReportName", Convert.ToInt32(clientId))?.Value;

            if (string.IsNullOrEmpty(reportName))
            {
                throw new Exception($"Fundraising Job Error: Report name configurations for clientId - {clientId} -  does not exits");
            }

            List<FundraisingOrdersModel> fundraisingOrders = GetFundraisingOrdersDataByClientId(clientId);

            string extension = "csv";
            if (fundraisingOrders != null && fundraisingOrders.Count() > 0)
            {
                Logger.Debug($"Generating Daily Fundraising Orders Report for date: {DateTime.Now:yyyyMMdd}, there are {fundraisingOrders.Count()} orders");
                fileName = $"{reportName}{clientId}-{DateTime.Now:yyyyMMdd}.{extension}";
            }

            if (!string.IsNullOrEmpty(fileName))
            {
                byte[] ordersFile = ExportToExcelHelper.ExportToExcel(fundraisingOrders, extension, true);
                new SendEmailHelper(_iconfiguration, Convert.ToInt32(clientId)).SendEmail("help@jifiti.com", SendEmailHelper.GetMessage(fundraisingOrders.Count()), "Daily Fundraising Orders Report", fileName, ordersFile);
            }
        }

        static List<FundraisingOrdersModel> GetFundraisingOrdersDataByClientId(string clientId)
        {
            Logger.Info("Get Fundraising Orders Data for Client Id " + clientId);
            var registryDAL = new RegistryDAL(_iconfiguration);
            int client = Convert.ToInt32(clientId);
            var fundraisingOrders = registryDAL.GetFundraisingOrders(client);
            return fundraisingOrders;
        }

        #region general
        static void GetAppSettingsFile()
        {
            Logger.Info("Get App Settings File");
            var builder = new ConfigurationBuilder()
                 .SetBasePath(Directory.GetCurrentDirectory())
                 .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
            _iconfiguration = builder.Build();
        }
        #endregion
    }
}
