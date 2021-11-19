using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WymianaKsiazek.Models.MapperModels;

namespace WymianaKsiazek.Queries.BookQueries
{
    public interface IBookQueries
    {
        public List<BookReviewsList> GetBooks();
        public List<BookReviewsList> GetBooksByCategory(long id);
        public List<BookReviewsList> GetBooksByAuthor(string author);
        public List<BookReviewsList> GetConcretBook(string title, string author);
        public List<BookReviewsList> GetSearchedBooks(string title, string author);
        public List<BookReviewsList> GetSearchedBooksByFilter(List<BookReviewsList> books, long categoryid, int sorttype);
        public BookOpinionMP GetBookById(long id);
        public BookOpinionMP GetBookOpinions(long bookid);
        public string GetCategoryName(long id);
        public int GetUserOpinionAboutBook(long opinionid, string userid);
        public Task AddUserOpinionAboutBook(int value, long opinionid, string userid);
    }
}
