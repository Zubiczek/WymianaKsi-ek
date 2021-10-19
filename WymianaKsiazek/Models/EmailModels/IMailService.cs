using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MailKit;
using MimeKit;

namespace WymianaKsiazek.Models.EmailModels
{
    public interface IMailService
    {
        Task SendEmailAsync(MailRequest mailrequest);
    }
}
