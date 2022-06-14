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
using Microsoft.AspNetCore.Authorization;

namespace WymianaKsiazek.Controllers
{
    [AllowAnonymous]
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

        public async Task<IActionResult> Index()
        {
            bool isuerloggedin = _loggedUser.IsUserLoggedIn();
            var offers = await _offerQueries.GetAllOffers();
            ViewBag.IsUserLoggedIn = isuerloggedin;
            if (isuerloggedin)
            {
                ViewBag.Username = HttpContext.Session.GetString("Username");
                ViewBag.UserImg = HttpContext.Session.GetString("UserImage");
            }
            return View(offers);
        }
        public async Task<IActionResult> Category(long id)
        {
            var categoryname = await _bookQueries.GetCategoryName(id);
            if (categoryname == null)
            {
                    return RedirectToAction("Error", "Home", new { statuscode = 404});
            }
            bool isuerloggedin = _loggedUser.IsUserLoggedIn();
            ViewBag.IsUserLoggedIn = isuerloggedin;
            if (isuerloggedin)
            {
                    ViewBag.Username = HttpContext.Session.GetString("Username");
                    ViewBag.UserImg = HttpContext.Session.GetString("UserImage");
            }
            ViewBag.CategoryName = categoryname;
            var books = await _bookQueries.GetBooksByCategory(id);
            var offers = await _offerQueries.GetOffersByCategory(id);
            dynamic model = new ExpandoObject();
            model.Offers = offers;
            model.Books = books;
            return View(model);
        }
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error(int? statuscode)
        {
            bool isuerloggedin = _loggedUser.IsUserLoggedIn();
            ViewBag.IsUserLoggedIn = isuerloggedin;
            if (isuerloggedin)
            {
                ViewBag.Username = HttpContext.Session.GetString("Username");
                ViewBag.UserImg = HttpContext.Session.GetString("UserImage");
            }
            ViewBag.StatusCode = statuscode;
            switch (statuscode)
            {
                case 404:
                    ViewBag.ErrorMessage = "Strona nie została odnaleziona!";
                    break;
                case 500:
                    ViewBag.ErrorMessage = "Błąd po stronie serwera!";
                    break;
                case 400:
                    ViewBag.ErrorMessage = "Niepoprawne dane użytkownika";
                    break;
                case 401:
                    ViewBag.ErrorMessage = "Błąd autoryzacji!";
                    break;
                default:
                    ViewBag.ErrorMessage = "Wystąpił niezidentyfikowany problem!";
                    break;
            }
            return View();
        }
    }
}
