using FundraisingOrdersJob.DAL;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Net.Mail;

namespace FundraisingOrdersJob.Helpers
{
    public class SendEmailHelper
    {
        private string _host { get; set; }
        private int _port { get; set; }
        private string _email { get; set; }
        private string _password { get; set; }
        private string _displayName { get; set; }
        private string _toEmails { get; set; }
        private string _fundraisingJobEmailFailure { get; set; }
        private int _clientId { get; set; }
        public SendEmailHelper(IConfiguration iconfiguration, int clientId = 0)
        {
            var dal = new RegistryDAL(iconfiguration);

            _clientId = clientId;
            _host = dal.GetConfigurations("FundraisingSMTPHost").Value;
            _port = Convert.ToInt32(dal.GetConfigurations("FundraisingSMTPPort").Value);
            _email = dal.GetConfigurations("FundraisingSMTPEmail").Value;
            _password = dal.GetConfigurations("FundraisingSMTPPassword").Value;
            _displayName = dal.GetConfigurations("FundraisingSMTPDisplayName").Value;
            _fundraisingJobEmailFailure = dal.GetConfigurations("FundraisingJobEmailFailure").Value;
            _toEmails = dal.GetConfigurations("FundraisingReceipientEmails", clientId)?.Value;
        }

        public void SendEmail(Exception exception, string subject)
        {
            SmtpClient smtpClient = new SmtpClient(_host, _port);
            smtpClient.UseDefaultCredentials = false;
            smtpClient.Credentials = new System.Net.NetworkCredential(_email, _password);
            smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;
            smtpClient.EnableSsl = true;

            MailMessage mail = new MailMessage
            {
                //Setting From , To and CC
                From = new MailAddress(_email, _displayName)
            };

            var emails = _fundraisingJobEmailFailure.Split(';');
            foreach (var email in emails)
            {
                if (!string.IsNullOrWhiteSpace(email))
                    mail.To.Add(new MailAddress(email));
            }

            mail.Subject = subject;
            mail.Body = GetMessage(exception);
            mail.IsBodyHtml = true;

            smtpClient.Send(mail);
        }

        public void SendEmail(string replyTo, string message, string subject, string fileName, byte[] ordersFile = null)
        {
            SmtpClient smtpClient = new SmtpClient(_host, _port);
            smtpClient.UseDefaultCredentials = false;
            smtpClient.Credentials = new System.Net.NetworkCredential(_email, _password);
            smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;
            smtpClient.EnableSsl = true;

            MailMessage mail = new MailMessage
            {
                From = new MailAddress("mail@jifiti.com", _displayName),
                Subject = subject,
                Body = message,
                IsBodyHtml = true
            };

            mail.ReplyToList.Add(replyTo);

            if (string.IsNullOrEmpty(_toEmails))
            {
                throw new Exception($"Fundraising Job Error: Receipient email configurations for clientId - {_clientId} -  does not exits");
            }

            var emails = _toEmails.Split(';');
            foreach (var toEmail in emails)
            {
                if (!string.IsNullOrWhiteSpace(toEmail))
                    mail.To.Add(new MailAddress(toEmail));
            }

            if (ordersFile != null)
            {
                Attachment att = new Attachment(new MemoryStream(ordersFile), fileName);
                mail.Attachments.Add(att);
            }

            smtpClient.Send(mail);
        }

        public static string GetMessage(int ordersCount)
        {
            string body = "Fundraising physical orders report file generated, there are " + ordersCount + " orders. <br />";
            body += "Please see the attachment. <br /><br />";
            body += "Thank you<br />";
            body += "Jifiti";

            return body;
        }

        public string GetMessage(Exception exception)
        {
            var innerExceptionMessage = exception.InnerException != null && !string.IsNullOrEmpty(exception.InnerException.Message) ? "InnerException Message: " + exception.InnerException?.Message + "<br />" : "";

            string body = "<h3>Fundraising Orders Job APP Failed</h3><br /><br />";
            body += "Error Message: " + exception.Message + "<br />";
            body += innerExceptionMessage;
            body += "Stack Trace: " + JsonConvert.SerializeObject(exception) + "<br /><br />";
            body += "Fundraising Orders Job APP";

            return body;
        }
    }
}
