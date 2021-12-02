using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WymianaKsiazek.Database;
using WymianaKsiazek.Functions;
using WymianaKsiazek.Models.MapperModels;
using WymianaKsiazek.Queries.BookQueries;
using WymianaKsiazek.Queries.OpinionQueries;
using WymianaKsiazek.Queries.UserQueries;

namespace WymianaKsiazek.Controllers
{
    public class BookController : Controller
    {
        private readonly ILogger<OfferController> _logger;
        private readonly IBookQueries _bookQueries;
        private readonly IOpinionQueries _opinionQueries;
        private readonly ILoggedInUser _loggedUser;
        public BookController(ILogger<OfferController> logger, IBookQueries bookQueries,
            IOpinionQueries opinionQueries, ILoggedInUser loggedUser)
        {
            _logger = logger;
            _bookQueries = bookQueries;
            _opinionQueries = opinionQueries;
            _loggedUser = loggedUser;
        }
        public async Task<IActionResult> Reviews(string author, string title)
        {
            List<BookReviewsList> books;
            if(author != null && title != null)
            {
                books = await _bookQueries.GetConcretBook(title, author);
            }
            else if(author != null)
            {
                books = await _bookQueries.GetBooksByAuthor(author);
            }
            else
            {
                books = await _bookQueries.GetBooks();
            }
            bool isuerloggedin = _loggedUser.IsUserLoggedIn();
            ViewBag.IsUserLoggedIn = isuerloggedin;
            if (isuerloggedin)
            {
                ViewBag.Username = HttpContext.Session.GetString("Username");
                ViewBag.UserImg = HttpContext.Session.GetString("UserImage");
            }
            TempData["currentbooks"] = JsonConvert.SerializeObject(books);
            return View(books);
        }
        public async Task<IActionResult> GetPartialBookSearchView(string inputsearchtitlebooks, string inputsearchauthorbook)
        {
            var books = await _bookQueries.GetSearchedBooks(inputsearchtitlebooks, inputsearchauthorbook);
            TempData["currentbooks"] = JsonConvert.SerializeObject(books);
            ViewBag.Title = inputsearchtitlebooks;
            ViewBag.Author = inputsearchauthorbook;
            return PartialView("PartialSearchBookView", books);
        }
        public IActionResult GetPartialBookSearchFilterView(string inputcategoryid, string inputtype)
        {
            if (TempData["currentbooks"] == null)
            {
                return RedirectToAction("SearchError", "Book");
            }
            var currentbooks = JsonConvert.DeserializeObject<List<BookReviewsList>>(TempData["currentbooks"].ToString());
            TempData["currentbooks"] = JsonConvert.SerializeObject(currentbooks);
            var books = _bookQueries.GetSearchedBooksByFilter(currentbooks, (long)Convert.ToDouble(inputcategoryid), Convert.ToInt32(inputtype));
            return PartialView("PartialSearchBookFilterView", books);
        }
        public async Task<IActionResult> ClearFilters()
        {
            var books = await _bookQueries.GetBooks();
            TempData["currentbooks"] = JsonConvert.SerializeObject(books);
            return PartialView("PartialSearchBookFilterView", books);
        }
        public IActionResult SearchError()
        {
            return PartialView("SearchError");
        }
        public async Task<IActionResult> book(long id)
        {
            var book = await _bookQueries.GetBookById(id);
            if(book == null)
            {
                return RedirectToAction("Error", "Home");
            }
            bool isuerloggedin = _loggedUser.IsUserLoggedIn();
            ViewBag.IsUserLoggedIn = isuerloggedin;
            if (isuerloggedin == true)
            {
                ViewBag.Username = HttpContext.Session.GetString("Username");
                ViewBag.UserImg = HttpContext.Session.GetString("UserImage");
                string userid = _loggedUser.GetUserId();
                if (userid == null || userid == "")
                {
                    return RedirectToAction("Error", "Home");
                }
                ViewBag.UserOpinion = await _opinionQueries.GetUserOpinionAboutBook(id, userid);
            }
            return View(book);
        }
    }
}
