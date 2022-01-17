using Log4NetLibrary;
using Microsoft.Extensions.Configuration;
using RegistryJob.DAL;
using Renci.SshNet;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace RegistryJob.Helpers
{
    public class FTPHelper
    {
        private string _sftpHost { get; set; }
        private string _sftpUserName { get; set; }
        private string _sftpPassword { get; set; }
        private string _sftpPort { get; set; }
        private string _sftpPath { get; set; }
        private int _clientId { get; set; }

        public FTPHelper(IConfiguration iconfiguration, int clientId = 0)
        {
            var dal = new RegistryDAL(iconfiguration);

            _clientId = clientId;
            _sftpHost = dal.GetConfigurations("RegSFTPHost", clientId).Value;
            _sftpPort = dal.GetConfigurations("RegSFTPPort", clientId).Value;
            _sftpUserName = dal.GetConfigurations("RegSFTPUserName", clientId).Value;
            _sftpPassword = dal.GetConfigurations("RegSFTPPassword", clientId).Value;
            _sftpPath = dal.GetConfigurations("RegSFTPPath", clientId).Value;
        }

        public void SendFileToSFTP(string pathToFile, string fileName)
        {
            string fullPath = pathToFile + fileName;

            string filename = Path.GetFileNameWithoutExtension(fullPath);

            if (File.Exists(fullPath))
            {
                Logger.Info("File found with name" + filename);
                DateTime creation = File.GetCreationTime(fullPath);

                if (creation.Day == DateTime.Now.Day && creation.Month == DateTime.Now.Month && creation.Year == DateTime.Now.Year)
                {
                    UploadFileToSFTP(fullPath, true);
                }
            }
            else
            {
                Logger.Error($"file {filename} does not exist!");
            }

            Logger.Info("Process Finished");

        }

        private void UploadFileToSFTP(string fullPath, bool clearFTPDirecotry = false)
        {
            try
            {
                // Upload File
                Logger.Info("Upload file to the SFTP");
                Console.WriteLine("Upload file to the SFTP");
                using (SftpClient sftp = new SftpClient(_sftpHost, Convert.ToInt32(_sftpPort), _sftpUserName, _sftpPassword))
                {
                    sftp.Connect();
                    sftp.ChangeDirectory(_sftpPath);

                    if (clearFTPDirecotry)
                    {
                        var files = sftp
                            .ListDirectory(".")
                            .Where(file => (file.Name != ".") && (file.Name != ".."))
                            .ToList();

                        foreach (var file in files)
                        {
                            sftp.DeleteFile(file.Name);
                        }
                    }

                    using (var uplfileStream = System.IO.File.OpenRead(fullPath))
                    {
                        sftp.UploadFile(uplfileStream, Path.GetFileName(fullPath), true);
                    }

                    sftp.Disconnect();
                }
            }
            catch (Exception ex)
            {
                Logger.Error("Error while uploading to FTP: " + ex.Message);
            }

            Logger.Info($"File sent to ftp");
        }
    }
}
