using System;
using System.Diagnostics;
using System.Net;
using System.Net.Mail;
using System.Text.RegularExpressions;
using SendGrid;
using SendGrid.Helpers.Mail;

namespace TimeTravel.API.Services
{
    public class LocalMailService : IMailService
    {
        private string _mailTo = Startup.Configuration["mailSettings:mailToAddress"];
        private string _mailFrom = Startup.Configuration["mailSettings:mailFromAddress"];


        public void Send(string subject, string message)
        {
            var mailMessage = new MailMessage
            {
                From = new MailAddress(_mailFrom),
                Subject = subject,
                Body = message
            };

            mailMessage.To.Add(_mailTo);

            var smtpClient = new SmtpClient
            {
                Credentials = new NetworkCredential("XXX", "XXX"),
                Host = "smtp.sendgrid.net",
                Port = 587
            };

            smtpClient.Send(mailMessage); 
        }

    }
}