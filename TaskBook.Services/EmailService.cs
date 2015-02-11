﻿using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using TaskBook.Services.Interfaces;

namespace TaskBook.Services
{
    /// <summary>
    /// The Mail Service based on the outlook.com host
    /// </summary>
    public sealed class EmailService: IEmailService
    {
        private readonly SmtpClient _smtpClient;

        /// <summary>
        /// Default constructor
        /// </summary>
        public EmailService()
        {
            _smtpClient = new SmtpClient("smtp-mail.outlook.com", 587);
        }

        /// <summary>
        /// Sends message to the outlook SMTP server for delivery
        /// </summary>
        /// <param name="message">Email message</param>
        public void Send(MailMessage message)
        {
            message.From = new MailAddress("b.obuhov@outlook.com");
            message.IsBodyHtml = true;

            _smtpClient.EnableSsl = true;
            _smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;
            _smtpClient.UseDefaultCredentials = false;
            _smtpClient.Credentials = new NetworkCredential("b.obuhov@outlook.com", "password");
            _smtpClient.Send(message);
        }

        /// <summary>
        /// Asyncronously sends message to the outlook SMYP server for delivery
        /// </summary>
        /// <param name="message">Email message</param>
        /// <returns>The task object representing the asynchronous operation</returns>
        public Task SendMailAsync(MailMessage message)
        {
            message.From = new MailAddress("b.obuhov@outlook.com");
            message.IsBodyHtml = true;

            _smtpClient.EnableSsl = true;
            _smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;
            _smtpClient.UseDefaultCredentials = false;
            _smtpClient.Credentials = new NetworkCredential("b.obuhov@outlook.com", "password");
            return _smtpClient.SendMailAsync(message);
        }

        public void Dispose()
        {
            _smtpClient.Dispose();
        }

    }
}
