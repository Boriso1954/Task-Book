using System;
using System.Net.Mail;
using System.Threading.Tasks;

namespace TaskBook.Services.Interfaces
{
    /// <summary>
    /// Defines the EmailService operations
    /// </summary>
    public interface IEmailService: IDisposable
    {
        /// <summary>
        /// Sends message to the outlook SMTP server for delivery
        /// </summary>
        /// <param name="message">Email message</param>
        void Send(MailMessage message);

        /// <summary>
        /// Asyncronously sends message to the outlook SMYP server for delivery
        /// </summary>
        /// <param name="message">Email message</param>
        /// <returns>The task object representing the asynchronous operation</returns>
        Task SendMailAsync(MailMessage message);
    }
}
