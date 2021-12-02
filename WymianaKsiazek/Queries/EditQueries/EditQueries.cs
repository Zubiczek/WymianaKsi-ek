using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WymianaKsiazek.Database;
using WymianaKsiazek.Database.Entities;

namespace WymianaKsiazek.Queries.EditQueries
{
    public class EditQueries : IEditQueries
    {
        private readonly Context _context;
        public EditQueries(Context context)
        {
            _context = context;
        }
        public async Task ChangeEmail(UserEntity user, string newemail)
        {
            if(user != null)
            {
                user.Email = newemail;
                _context.Update(user);
                await _context.SaveChangesAsync();
            }
        }
        public async Task ChangeUsername(UserEntity user, string newusername)
        {
            if (user != null)
            {
                user.UserName = newusername;
                _context.Update(user);
                await _context.SaveChangesAsync();
            }
        }
        public async Task ChangeImage(UserEntity user, string imagename)
        {
            if (user != null)
            {
                user.Img = imagename;
                _context.Update(user);
                await _context.SaveChangesAsync();
            }
        }
    }
}
