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

namespace WymianaKsiazek.Controllers
{
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
        public IActionResult offer(long id)
        {
            var offer = _offerQueries.GetOfferById(id);
            var isuserloggedin = _loggedUser.IsUserLoggedIn();
            ViewBag.IsUserLoggedIn = isuserloggedin;
            if (offer == null)
            {
                return RedirectToAction("Error", "Home");
            }
            if(isuserloggedin)
            {
                ViewBag.Username = HttpContext.Session.GetString("Username");
                ViewBag.UserImg = HttpContext.Session.GetString("UserImage");
                string userid = _loggedUser.GetUserId();
                if(userid == null || userid == "")
                {
                    return RedirectToAction("Error", "Home");
                }
                ViewBag.IsOfferLikedByUser = _likeQueries.IsOfferFollowedByUser(id, userid);
            }
            return View(offer);
        }
        public IActionResult search(string title, string author)
        {
            List<OffersListMP> offers;
            if (title != null && author != null)
            {
                offers = _offerQueries.GetOffersWithBook(title, author);
            }
            else
            {
                offers = _offerQueries.GetOffers();
            }
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
        public IActionResult GetPartialSearchView(string searchbookbox, string searchlocationbox)
        {
            var offers = _offerQueries.GetSearchesOffers(searchbookbox, searchlocationbox);
            TempData["currentoffers"] = JsonConvert.SerializeObject(offers);
            ViewBag.TitleorAuthor = searchbookbox;
            ViewBag.Location = searchlocationbox;
            return PartialView("PartialSearchView", offers);
        }
        public IActionResult GetPartialSearchByFilterView(string inputcategoryid, string inputlowprice, string inputupprice, string inputtype)
        {
            if (TempData["currentoffers"] == null)
            {
                return RedirectToAction("SearchError", "Offer");
            }
            var currentoffers = JsonConvert.DeserializeObject<List<OffersListMP>>(TempData["currentoffers"].ToString());
            TempData["currentoffers"] = JsonConvert.SerializeObject(currentoffers);
            var offers = _offerQueries.GetSearchedOffersByFilters(currentoffers, (long)Convert.ToDouble(inputcategoryid), Convert.ToUInt32(inputlowprice), Convert.ToUInt32(inputupprice), Convert.ToInt32(inputtype));
            return PartialView("PartialSearchFilterView", offers);
        }
        public IActionResult ClearFilters()
        {
            var offers = _offerQueries.GetOffers();
            TempData["currentoffers"] = JsonConvert.SerializeObject(offers);
            return PartialView("PartialClearFiltersView", offers);
        }
        public IActionResult SearchError()
        {
            return PartialView("SearchError");
        }
        public IActionResult AddOffer()
        {
            bool isuerloggedin = _loggedUser.IsUserLoggedIn();
            ViewBag.IsUserLoggedIn = isuerloggedin;
            if(!isuerloggedin)
            {
                return RedirectToAction("Login", "User");
            }
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
        public IActionResult GetBooks(string title)
        {
            var books = _offerQueries.GetBooksToAdd(title);
            return Json(books);
        }
        [HttpPost]
        public async Task<IActionResult> AddNewOffer(AddOffer model)
        {
            string userid = _loggedUser.GetUserId();
            if(userid == null || userid == "")
            {
                return RedirectToAction("Error", "Home");
            }
            int result = await _offerQueries.AddOffer(model, userid);
            if(result == 1)
            {
                TempData["addoffererror"] = "Opis zawiera niedozwolone słowa!";
                return RedirectToAction("AddOffer", "Offer");
            }
            else if(result == 2)
            {
                return RedirectToAction("Error", "Home");
            }
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
