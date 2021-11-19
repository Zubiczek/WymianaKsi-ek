using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WymianaKsiazek.Database;
using WymianaKsiazek.Models.MapperModels;
using WymianaKsiazek.Queries.BookQueries;

namespace WymianaKsiazek.Controllers
{
    public class BookController : Controller
    {
        private readonly ILogger<OfferController> _logger;
        private readonly IBookQueries _bookQueries;
        private readonly Context _context;
        public BookController(ILogger<OfferController> logger, IBookQueries bookQueries, Context context)
        {
            _logger = logger;
            _bookQueries = bookQueries;
            _context = context;
        }
        public IActionResult Reviews(string author, string title)
        {
            List<BookReviewsList> books;
            if(author != null && title != null)
            {
                books = _bookQueries.GetConcretBook(title, author);
            }
            else if(author != null)
            {
                books = _bookQueries.GetBooksByAuthor(author);
            }
            else
            {
                books = _bookQueries.GetBooks();
            }
            //var books = _bookQueries.GetBooks();
            bool isuerloggedin = IsUserLoggedIn();
            ViewBag.IsUserLoggedIn = isuerloggedin;
            TempData["currentbooks"] = JsonConvert.SerializeObject(books);
            return View(books);
        }
        public IActionResult GetPartialBookSearchView(string inputsearchtitlebooks, string inputsearchauthorbook)
        {
            var books = _bookQueries.GetSearchedBooks(inputsearchtitlebooks, inputsearchauthorbook);
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
        public IActionResult ClearFilters()
        {
            var books = _bookQueries.GetBooks();
            TempData["currentbooks"] = JsonConvert.SerializeObject(books);
            return PartialView("PartialSearchBookFilterView", books);
        }
        public IActionResult SearchError()
        {
            return PartialView("SearchError");
        }
        public IActionResult book(long id)
        {
            var book = _bookQueries.GetBookById(id);
            if(book == null)
            {
                return RedirectToAction("Error", "Home");
            }
            bool ifuserloggedin = IsUserLoggedIn();
            ViewBag.IsUserLoggedIn = ifuserloggedin;
            if(ifuserloggedin == true)
            {
                string userid = GetUserId();
                if (userid == null || userid == "")
                {
                    return RedirectToAction("Error", "Home");
                }
                ViewBag.UserOpinion = _bookQueries.GetUserOpinionAboutBook(id, userid);
            }
            return View(book);
        }
        [HttpPost]
        public async Task<IActionResult> AddOpinion(int value, string opinionid)
        {
            long opinion_id = (long)Convert.ToDouble(opinionid);
            if (IsUserLoggedIn() == false)
            {
                return Json(new { success = false, responseText = "Unauthorized!"});
            }
            else
            {
                string userid = GetUserId();
                if (userid == null || userid == "")
                {
                    return Json(new { success = false, responseText = "Server Error!" });
                }
                await _bookQueries.AddUserOpinionAboutBook(value, opinion_id, userid);
                return Json(new { success = true });
            }
        }
        private bool IsUserLoggedIn()
        {
            bool p = false;
            string token = HttpContext.Session.GetString("Token");
            if (token != null)
            {
                p = true;
            }
            return p;
        }
        private string GetUserId()
        {
            var refreshtoken = HttpContext.Session.GetString("RefreshToken");
            string userid = _context.RefreshTokens.Where(x => x.Token == refreshtoken).Select(x => x.UserId).FirstOrDefault();
            return userid;
        }
    }
}
