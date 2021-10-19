using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WymianaKsiazek.Models.MapperModels;

namespace WymianaKsiazek.Queries.BookQueries
{
    public interface IBookQueries
    {
        public BookOpinionMP GetBookOpinions(long bookid);
    }
}
