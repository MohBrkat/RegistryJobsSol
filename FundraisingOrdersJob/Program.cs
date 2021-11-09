﻿using DailyReigstryDetailsJob.DAL;
using DailyReigstryDetailsJob.Helpers;
using DailyReigstryDetailsJob.Models;
using Log4NetLibrary;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace DailyReigstryDetailsJob
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
                var clientIdsString = "273";
                //var clientIdsString = registryDAL.GetConfigurations("DailyRegistryDetailsJobClientIds")?.Value;

                List<string> clientIdList = clientIdsString.Split(',').ToList();
                foreach (var clientId in clientIdList)
                {
                    GenerateDailyRegistryDetailsReport(clientId);
                }

            }
            catch (Exception ex)
            {
                Logger.Error(ex.Message);
                new SendEmailHelper(_iconfiguration).SendEmail(ex, "Daily Registry Details Job Error: Job Failed.");
            }
        }

        private static void GenerateDailyRegistryDetailsReport(string clientId)
        {
            string fileName = string.Empty;

            var registryDAL = new RegistryDAL(_iconfiguration);
            string reportName = registryDAL.GetConfigurations("DailyRegistryDetailsJobReportName", Convert.ToInt32(clientId))?.Value;

            if (string.IsNullOrEmpty(reportName))
            {
                throw new Exception($"Daily Registry Details Job Error: Report name configurations for clientId - {clientId} -  does not exits");
            }

            List<DailyReigstryDetailsModel> dailyReigstryDetails = GetDailyRegistryDetailsByClientId(clientId);

            string extension = "csv";
            if (dailyReigstryDetails != null && dailyReigstryDetails.Count() > 0)
            {
                Logger.Debug($"Generating Daily Reigstry Details Report for date: {DateTime.Now:yyyyMMdd}, there are {dailyReigstryDetails.Count()} orders");
                fileName = $"{reportName}{clientId}-{DateTime.Now:yyyyMMdd}.{extension}";
            }

            if (!string.IsNullOrEmpty(fileName))
            {
                byte[] ordersFile = ExportToExcelHelper.ExportToExcel(dailyReigstryDetails, extension, true);
                new SendEmailHelper(_iconfiguration, Convert.ToInt32(clientId)).SendEmail("help@jifiti.com", SendEmailHelper.GetMessage(dailyReigstryDetails.Count()), "Daily Reigstry Details Report", fileName, ordersFile);
            }
        }

        static List<DailyReigstryDetailsModel> GetDailyRegistryDetailsByClientId(string clientId)
        {
            Logger.Info("Get Daily Registry Details Data for Client Id " + clientId);
            var registryDAL = new RegistryDAL(_iconfiguration);
            int client = Convert.ToInt32(clientId);
            var dailyReigstryDetails = registryDAL.GetDailyRegistryDetailsData(client);
            return dailyReigstryDetails;
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