using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WymianaKsiazek.Queries.OfferQueries;
using Microsoft.AspNetCore.Http;
using WymianaKsiazek.Models;
using WymianaKsiazek.Models.MapperModels;
using System.Text.Json;
using System.Text.Json.Serialization;
using Newtonsoft.Json;
using WymianaKsiazek.Database;
using System.Diagnostics;
using WymianaKsiazek.Queries.UserQueries;
using WymianaKsiazek.Queries.LikeQueries;
using WymianaKsiazek.Functions;
using Microsoft.AspNetCore.Authorization;

namespace WymianaKsiazek.Controllers
{
    [AllowAnonymous]
    public class OfferController : Controller
    {
        private readonly ILogger<OfferController> _logger;
        private readonly IOfferQueries _offerQueries;
        private readonly ILikeQueries _likeQueries;
        private readonly ILoggedInUser _loggedUser;
        public OfferController(ILogger<OfferController> logger, IOfferQueries offerQueries,
            ILikeQueries likeQueries, ILoggedInUser loggedUser)
        {
            _logger = logger;
            _offerQueries = offerQueries;
            _likeQueries = likeQueries;
            _loggedUser = loggedUser;
        }
        public async Task<IActionResult> offer(long id)
        {
            var offer = await _offerQueries.GetOfferById(id);
            if (offer == null)  return RedirectToAction("Error", "Home", new { statuscode = 404 });
            var isuserloggedin = _loggedUser.IsUserLoggedIn();
            ViewBag.IsUserLoggedIn = isuserloggedin;
            if (isuserloggedin)
            {
                ViewBag.Username = HttpContext.Session.GetString("Username");
                ViewBag.UserImg = HttpContext.Session.GetString("UserImage");
                string userid = _loggedUser.GetUserId();
                if (userid == null || userid == "") return RedirectToAction("Error", "Home", new { statuscode = 401 });
                ViewBag.IsOfferLikedByUser = await _likeQueries.IsOfferFollowedByUser(id, userid);
            }
            return View(offer);
        }
        public async Task<IActionResult> search(string title, string author)
        {
            List<OffersListMP> offers;
            if (title != null && author != null)    offers = await _offerQueries.GetOffersWithBook(title, author);
            else offers = await _offerQueries.GetOffers();
            TempData["currentoffers"] = JsonConvert.SerializeObject(offers);
            bool isuerloggedin = _loggedUser.IsUserLoggedIn();
            ViewBag.IsUserLoggedIn = isuerloggedin;
            if (isuerloggedin)
            {
                ViewBag.Username = HttpContext.Session.GetString("Username");
                ViewBag.UserImg = HttpContext.Session.GetString("UserImage");
            }
            return View(offers);
        }
        public async Task<IActionResult> GetPartialSearchView(string searchbookbox, string searchlocationbox)
        {
            var offers = await _offerQueries.GetSearchesOffers(searchbookbox, searchlocationbox);
            TempData["currentoffers"] = JsonConvert.SerializeObject(offers);
            ViewBag.TitleorAuthor = searchbookbox;
            ViewBag.Location = searchlocationbox;
            return PartialView("PartialSearchView", offers);
        }
        public IActionResult GetPartialSearchByFilterView(string inputcategoryid, string inputlowprice, string inputupprice, string inputtype)
        {
            if (TempData["currentoffers"] == null)  return RedirectToAction("SearchError", "Offer");
            var currentoffers = JsonConvert.DeserializeObject<List<OffersListMP>>(TempData["currentoffers"].ToString());
            TempData["currentoffers"] = JsonConvert.SerializeObject(currentoffers);
            var offers = _offerQueries.GetSearchedOffersByFilters(currentoffers, (long)Convert.ToDouble(inputcategoryid), Convert.ToUInt32(inputlowprice), Convert.ToUInt32(inputupprice), Convert.ToInt32(inputtype));
            return PartialView("PartialSearchFilterView", offers);
        }
        public async Task<IActionResult> ClearFilters()
        {
            var offers = await _offerQueries.GetOffers();
            TempData["currentoffers"] = JsonConvert.SerializeObject(offers);
            return PartialView("PartialClearFiltersView", offers);
        }
        public IActionResult SearchError()
        {
            return PartialView("SearchError");
        }
        [Authorize]
        public IActionResult AddOffer()
        {
            bool isuerloggedin = _loggedUser.IsUserLoggedIn();
            ViewBag.IsUserLoggedIn = isuerloggedin;
            if (!isuerloggedin) return RedirectToAction("Login", "User");
            else
            {
                ViewBag.Username = HttpContext.Session.GetString("Username");
                ViewBag.UserImg = HttpContext.Session.GetString("UserImage");
            }
            ViewBag.Error = TempData["addoffererror"];
            TempData["addoffererror"] = null;
            return View();
        }
        [HttpGet]
        public async Task<IActionResult> GetBooks(string title)
        {
            var books = await _offerQueries.GetBooksToAdd(title);
            return Json(books);
        }
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> AddNewOffer(AddOffer model)
        {
            string userid = _loggedUser.GetUserId();
            if (userid == null || userid == "") return RedirectToAction("Error", "Home", new { statuscode = 401 });
            int result = await _offerQueries.AddOffer(model, userid);
            if (result == 1)
            {
                TempData["addoffererror"] = "Opis zawiera niedozwolone słowa!";
                return RedirectToAction("AddOffer", "Offer");
            }
            else if (result == 2)   return RedirectToAction("Error", "Home", new { statuscode = 401 });
            else if (result == 3)
            {
                TempData["addoffererror"] = "Osiągnięto limit ogłoszeń!";
                return RedirectToAction("AddOffer", "Offer");
            }
            else if (result == 4)
            {
                TempData["addoffererror"] = "Brak danej książki w naszej bazie!";
                return RedirectToAction("AddOffer", "Offer");
            }
            return RedirectToAction("MyProfile", "User");
        }
    }
}
