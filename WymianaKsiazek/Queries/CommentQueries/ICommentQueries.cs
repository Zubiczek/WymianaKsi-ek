using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WymianaKsiazek.Queries.CommentQueries
{
    public interface ICommentQueries
    {
        public Task AddCommentToOffer(string comment, string userid, long offerid);
        public Task AddCommentToBook(string comment, string userid, long bookid);
    }
}
