using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WymianaKsiazek.Database.Entities;
using WymianaKsiazek.Models.EmailModels;

namespace WymianaKsiazek.Queries.EmailQueries
{
    public class EmailQueries : IEmailQueries
    {
        private readonly IMailService _mailService;
        private readonly IUrlHelper _urlHelper;
        private HttpRequest _request;
        public EmailQueries(IMailService mailService, IUrlHelper urlHelper, IHttpContextAccessor httpContextAccessor)
        {
            _mailService = mailService;
            _urlHelper = urlHelper;
            _request = httpContextAccessor.HttpContext.Request;
        }
        public async Task SendEmailVerification(UserEntity user, string token)
        {
            var confirmationlink = _urlHelper.Action("ConfirmEmail", "User", new { userId = user.Id, token = token }, _request.Scheme);
            MailRequest mailRequest = new MailRequest();
            mailRequest.ToEmail = user.Email;
            mailRequest.Subject = "wymienksiazke.com - Weryfikacja konta";
            string mailtext = "<div style = 'background-color: #696969; color: white; margin-left: auto; margin-right: auto; width: 100%;'>" +
            "<div style = 'padding-top: 30px; text-align: center;'>" +
                "Witaj, Twoje konto jest prawie gotowe!" +
            "</div>" +
            "<div style = 'padding-top: 30px; text-align: center;'>" +
                "Kliknij poniższy przycisk aby aktywować swoje konto" +
            "</div>" +
            "<div style = 'background-color: #228B22; padding: 10px; width: 100px; margin-left: 42%; margin-top: 30px;'>" +
                "<a href = '" + confirmationlink + "' style = 'text-decoration:none'>" +
                    "<div style = 'text-align: center; color: white;'>" +
                        "Weryfikuj" +
                    "</div>" +
                "</a>" +
            "</div>" +
            "<div style = 'background-color: black; text-align: center; margin-top: 20px; color: white;'>" +
                "wymienksiazke.com &copy Wszelkie prawa zastrzeżone!" +
            "</div>" +
            "</div>";
            mailRequest.Body = mailtext;
            await _mailService.SendEmailAsync(mailRequest);
        }
        public async Task SendEmailResetPassword(UserEntity user, string token)
        {
            var confirmationlink = _urlHelper.Action("ResetPassword", "User", new { userId = user.Id, token = token }, _request.Scheme);
            MailRequest mailRequest = new MailRequest();
            mailRequest.ToEmail = user.Email;
            mailRequest.Subject = "wymienksiazke.com - Reset hasła - Weryfikacja";
            string mailtext = "<div style = 'background-color: #696969; color: white; margin-left: auto; margin-right: auto; width: 100%;'>" +
            "<div style = 'padding-top: 30px; text-align: center;'>" +
                "Reset hasła - Weryfikacja!" +
            "</div>" +
            "<div style = 'padding-top: 30px; text-align: center;'>" +
                "Kliknij poniższy przycisk zresetować hasło" +
            "</div>" +
            "<div style = 'background-color: #228B22; padding: 10px; width: 100px; margin-left: 42%; margin-top: 30px;'>" +
                "<a href = '" + confirmationlink + "' style = 'text-decoration:none'>" +
                    "<div style = 'text-align: center; color: white;'>" +
                        "Resetuj" +
                    "</div>" +
                "</a>" +
            "</div>" +
            "<div style = 'background-color: black; text-align: center; margin-top: 20px; color: white;'>" +
                "wymienksiazke.com &copy Wszelkie prawa zastrzeżone!" +
            "</div>" +
            "</div>";
            mailRequest.Body = mailtext;
            await _mailService.SendEmailAsync(mailRequest);
        }
    }
}
