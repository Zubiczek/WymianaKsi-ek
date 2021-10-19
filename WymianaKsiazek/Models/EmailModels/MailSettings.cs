using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WymianaKsiazek.Models.EmailModels
{
    public class MailSettings
    {
        public string Mail { get; set; }
        public string DisplayName { get; set; }
        public string Password { get; set; }
        public string Host { get; set; }
        public int Port { get; set; }
        public MailSettings()
        {
            Mail = "wymienksiazke99@gmail.com";
            DisplayName = "Wymien Ksiazke Serwis";
            Password = "zespol10UMK!";
            Host = "smtp.gmail.com";
            Port = 587;
        }
    }
}
