using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WymianaKsiazek.Models.MapperModels;

namespace WymianaKsiazek.Queries.BookQueries
{
    public interface IBookQueries
    {
        Task<List<BookReviewsList>> GetBooks();
        Task<List<BookReviewsList>> GetBooksByCategory(long id);
        Task<List<BookReviewsList>> GetBooksByAuthor(string author);
        Task<List<BookReviewsList>> GetConcretBook(string title, string author);
        Task<List<BookReviewsList>> GetSearchedBooks(string title, string author);
        List<BookReviewsList> GetSearchedBooksByFilter(List<BookReviewsList> books, long categoryid, int sorttype);
        Task<BookOpinionMP> GetBookById(long id);
        Task<BookOpinionMP> GetBookOpinions(long bookid);
        Task<string> GetCategoryName(long id);
    }
}
