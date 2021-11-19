using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using WymianaKsiazek.Database;
using WymianaKsiazek.Models;
using WymianaKsiazek.Queries.OfferQueries;
using WymianaKsiazek.Database.Entities;
using WymianaKsiazek.Queries.BookQueries;
using System.Dynamic;
using WymianaKsiazek.Queries.UserQueries;

namespace WymianaKsiazek.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly Context _context;
        private readonly IOfferQueries _offerQueries;
        private readonly IBookQueries _bookQueries;
        private readonly IUserQueries _userQueries;

        public HomeController(ILogger<HomeController> logger, Context context, IOfferQueries offerQueries, IBookQueries bookQueries
            , IUserQueries userQueries)
        {
            _logger = logger;
            _context = context;
            _offerQueries = offerQueries;
            _bookQueries = bookQueries;
            _userQueries = userQueries;
        }

        public IActionResult Index()
        {
            bool isuerloggedin = IsUserLoggedIn();
            var offers = _offerQueries.GetAllOffers();
            ViewBag.IsUserLoggedIn = isuerloggedin;
            return View(offers);
        }
        public IActionResult Category(long id)
        {
            var categoryname = _bookQueries.GetCategoryName(id);
            if(categoryname == null)
            {
                return RedirectToAction("Error", "Home");
            }
            bool isuerloggedin = IsUserLoggedIn();
            ViewBag.IsUserLoggedIn = isuerloggedin;
            ViewBag.CategoryName = categoryname;
            var books = _bookQueries.GetBooksByCategory(id);
            var offers = _offerQueries.GetOffersByCategory(id);
            dynamic model = new ExpandoObject();
            model.Offers = offers;
            model.Books = books;
            return View(model);
        }
        public IActionResult Test()
        {
            ViewBag.IsUserLoggedIn = IsUserLoggedIn();
            var offer = _offerQueries.GetOfferById(1);
            return View(offer);
        }
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            ViewBag.IsUserLoggedIn = IsUserLoggedIn();
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
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
