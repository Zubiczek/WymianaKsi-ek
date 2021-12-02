using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WymianaKsiazek.Database.Entities;

namespace WymianaKsiazek.Queries.EditQueries
{
    public interface IEditQueries
    {
        Task ChangeEmail(UserEntity user, string newemail);
        Task ChangeUsername(UserEntity user, string newusername);
        Task ChangeImage(UserEntity user, string imagename);
    }
}
