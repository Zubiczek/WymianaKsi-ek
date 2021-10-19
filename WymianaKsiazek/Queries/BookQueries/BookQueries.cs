using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WymianaKsiazek.Database;
using WymianaKsiazek.Models.MapperModels;
using Microsoft.EntityFrameworkCore;

namespace WymianaKsiazek.Queries.BookQueries
{
    public class BookQueries
    {
        private readonly IMapper _mapper;
        private readonly Context _context;
        public BookQueries(IMapper mapper, Context context)
        {
            _mapper = mapper;
            _context = context;
        }
        public BookOpinionMP GetBookOpinions(long bookid)
        {
            var bookopinion = _context.Book.Include(x => x.Category).Include(x => x.Offers).Include(x => x.Comments)
                .Where(x => x.Book_Id == bookid).FirstOrDefault();
            return _mapper.Map<BookOpinionMP>(bookopinion);
        }
    }
}
