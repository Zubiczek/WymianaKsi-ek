using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WymianaKsiazek.Models.MapperModels;

namespace WymianaKsiazek.Queries.OpinionQueries
{
    public interface IOpinionQueries
    {
        public int GetUserOpinionAboutUser(string userprofileid, string usersopinionid);
        public Task AddUserOpinionAboutUser(int value, long opinion_id, string userid);
        public Task<int> GetUserOpinionAboutBook(long opinionid, string userid);
        public Task AddUserOpinionAboutBook(int value, long opinionid, string userid);
    }
}
