using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WymianaKsiazek.Database;
using WymianaKsiazek.Database.Entities;

namespace WymianaKsiazek.Queries.CommentQueries
{
    public class CommentQueries : ICommentQueries
    {
        private readonly Context _context;
        private readonly IMapper _mapper;
        public CommentQueries(Context context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task AddCommentToOffer(string comment, string userid, long offerid)
        {
            OfferCommentsEntity commententity = new OfferCommentsEntity();
            using (var transaction = _context.Database.BeginTransaction())
            {
                commententity.Content = comment;
                commententity.User_Id = userid;
                commententity.Offer_Id = offerid;
                _context.OfferComments.Add(commententity);
                await _context.SaveChangesAsync();
                transaction.Commit();
            }
        }
        public async Task AddCommentToBook(string comment, string userid, long bookid)
        {
            BookCommentsEntity newbook = new BookCommentsEntity();
            using (var transaction = _context.Database.BeginTransaction())
            {
                newbook.Content = comment;
                newbook.User_Id = userid;
                newbook.Book_Id = bookid;
                _context.BookComments.Add(newbook);
                await _context.SaveChangesAsync();
                transaction.Commit();
            }
        }
    }
}
