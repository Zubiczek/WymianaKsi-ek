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
using WymianaKsiazek.Functions;

namespace WymianaKsiazek.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IOfferQueries _offerQueries;
        private readonly IBookQueries _bookQueries;
        private readonly ILoggedInUser _loggedUser;

        public HomeController(ILogger<HomeController> logger, IOfferQueries offerQueries, IBookQueries bookQueries,
            ILoggedInUser loggedUser)
        {
            _logger = logger;
            _offerQueries = offerQueries;
            _bookQueries = bookQueries;
            _loggedUser = loggedUser;
        }

        public IActionResult Index()
        {
            bool isuerloggedin = _loggedUser.IsUserLoggedIn();
            var offers = _offerQueries.GetAllOffers();
            ViewBag.IsUserLoggedIn = isuerloggedin;
            if (isuerloggedin)
            {
                ViewBag.Username = HttpContext.Session.GetString("Username");
                ViewBag.UserImg = HttpContext.Session.GetString("UserImage");
            }
            return View(offers);
        }
        public IActionResult Category(long id)
        {
            var categoryname = _bookQueries.GetCategoryName(id);
            if(categoryname == null)
            {
                return RedirectToAction("Error", "Home");
            }
            bool isuerloggedin = _loggedUser.IsUserLoggedIn();
            ViewBag.IsUserLoggedIn = isuerloggedin;
            if (isuerloggedin)
            {
                ViewBag.Username = HttpContext.Session.GetString("Username");
                ViewBag.UserImg = HttpContext.Session.GetString("UserImage");
            }
            ViewBag.CategoryName = categoryname;
            var books = _bookQueries.GetBooksByCategory(id);
            var offers = _offerQueries.GetOffersByCategory(id);
            dynamic model = new ExpandoObject();
            model.Offers = offers;
            model.Books = books;
            return View(model);
        }
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            bool isuerloggedin = _loggedUser.IsUserLoggedIn();
            ViewBag.IsUserLoggedIn = isuerloggedin;
            if (isuerloggedin)
            {
                ViewBag.Username = HttpContext.Session.GetString("Username");
                ViewBag.UserImg = HttpContext.Session.GetString("UserImage");
            }
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
