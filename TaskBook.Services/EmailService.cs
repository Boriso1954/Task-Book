using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using TaskBook.Services.Interfaces;

namespace TaskBook.Services
{
    public sealed class EmailService: IEmailService
    {
        private readonly SmtpClient _smtpClient;

        public EmailService()
        {
            _smtpClient = new SmtpClient("smtp-mail.outlook.com", 587);
        }

        public void Send(MailMessage message)
        {
            message.From = new MailAddress("b.obuhov@outlook.com");
            message.IsBodyHtml = true;

            _smtpClient.EnableSsl = true;
            _smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;
            _smtpClient.UseDefaultCredentials = false;
            _smtpClient.Credentials = new NetworkCredential("b.obuhov@outlook.com", "bMo#1962");
            _smtpClient.Send(message);
        }

        public Task SendMailAsync(MailMessage message)
        {
            message.From = new MailAddress("b.obuhov@outlook.com");
            message.IsBodyHtml = true;

            _smtpClient.EnableSsl = true;
            _smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;
            _smtpClient.UseDefaultCredentials = false;
            _smtpClient.Credentials = new NetworkCredential("b.obuhov@outlook.com", "bMo#1962");
            return _smtpClient.SendMailAsync(message);
        }

        public void Dispose()
        {
            _smtpClient.Dispose();
        }

    }
}
