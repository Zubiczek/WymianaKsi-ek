using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WymianaKsiazek.Database;
using WymianaKsiazek.Database.Entities;
using Microsoft.EntityFrameworkCore;

namespace WymianaKsiazek.Queries.OpinionQueries
{
    public class OpinionQueries : IOpinionQueries
    {
        private readonly Context _context;
        public OpinionQueries(Context context)
        {
            _context = context;
        }
        public int GetUserOpinionAboutUser(string userprofileid, string usersopinionid)
        {
            var opinionid = _context.UserOpinions.Where(x => x.User_Id == userprofileid).Select(x => x.Opinion_Id).FirstOrDefault();
            if (opinionid == 0)
            {
                return -1;
            }
            var useropinion = 0;
            useropinion = _context.UserOpinionInfo.Where(x => x.Opinion_Id == opinionid && x.User_Id == usersopinionid).Select(x => x.Value).FirstOrDefault();
            return useropinion;
        }
        public async Task AddUserOpinionAboutUser(int value, long opinion_id, string userid)
        {
            UserUserOpinionEntity useropinion = new UserUserOpinionEntity();
            useropinion.User_Id = userid;
            useropinion.Opinion_Id = opinion_id;
            useropinion.Value = value;
            using (var transaction = _context.Database.BeginTransaction())
            {
                _context.Add(useropinion);
                var useropiniontable = _context.UserOpinions.Where(x => x.Opinion_Id == opinion_id).FirstOrDefault();
                if (useropiniontable != null)
                {
                    useropiniontable.OpinionSumValue += (uint)value;
                    useropiniontable.TotalOpinions += 1;
                }
                await _context.SaveChangesAsync();
                transaction.Commit();
            }
        }
        public async Task<int> GetUserOpinionAboutBook(long bookid, string userid)
        {
            int value = 0;
            value = await _context.UserBookOpinions.Where(x => x.Opinion.Book_Id == bookid && x.User_Id == userid).Select(x => x.Value).FirstOrDefaultAsync();
            return value;
        }
        public async Task AddUserOpinionAboutBook(int value, long opinionid, string userid)
        {
            UserBookOpinion opinion = new UserBookOpinion();
            opinion.Opinion_Id = opinionid;
            opinion.User_Id = userid;
            opinion.Value = value;
            using (var transaction = _context.Database.BeginTransaction())
            {
                _context.Add(opinion);
                var bookopinion = _context.BookOpinions.Where(x => x.Opinion_Id == opinionid).FirstOrDefault();
                if (bookopinion != null)
                {
                    if (value == 1)
                    {
                        bookopinion.Opinion1 += 1;
                    }
                    else if (value == 2)
                    {
                        bookopinion.Opinion2 += 1;
                    }
                    else if (value == 3)
                    {
                        bookopinion.Opinion3 += 1;
                    }
                    else if (value == 4)
                    {
                        bookopinion.Opinion4 += 1;
                    }
                    else if (value == 5)
                    {
                        bookopinion.Opinion5 += 1;
                    }
                }
                await _context.SaveChangesAsync();
                transaction.Commit();
            }
        }
    }
}
