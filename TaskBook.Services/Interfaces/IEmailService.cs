using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace TaskBook.Services.Interfaces
{
    public interface IEmailService: IDisposable
    {
        void Send(MailMessage message);
        Task SendMailAsync(MailMessage message);
    }
}
