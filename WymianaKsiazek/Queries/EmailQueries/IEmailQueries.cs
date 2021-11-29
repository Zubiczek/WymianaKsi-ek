using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WymianaKsiazek.Database.Entities;

namespace WymianaKsiazek.Queries.EmailQueries
{
    public interface IEmailQueries
    {
        public Task SendEmailVerification(UserEntity user, string token);
        public Task SendEmailResetPassword(UserEntity user, string token);
    }
}
