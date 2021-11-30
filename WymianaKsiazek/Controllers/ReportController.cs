using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WymianaKsiazek.Functions;
using WymianaKsiazek.Queries.OfferQueries;
using WymianaKsiazek.Queries.ReportQueries;
using WymianaKsiazek.Queries.UserQueries;

namespace WymianaKsiazek.Controllers
{
    public class ReportController : Controller
    {
        private readonly ILoggedInUser _loggedUser;
        private readonly IOfferQueries _offerQueries;
        private readonly IUserQueries _userQueries;
        private readonly IReportQueries _reportQueries;
        public ReportController(ILoggedInUser loggedUser, IOfferQueries offerQueries, IUserQueries userQueries, 
            IReportQueries reportQueries)
        {
            _loggedUser = loggedUser;
            _offerQueries = offerQueries;
            _userQueries = userQueries;
            _reportQueries = reportQueries;
        }
        public async Task<IActionResult> Overture(long id)
        {
            bool isuerloggedin = _loggedUser.IsUserLoggedIn();
            ViewBag.IsUserLoggedIn = isuerloggedin;
            if (!isuerloggedin)
            {
                return RedirectToAction("Login", "User");
            }
            else
            {
                ViewBag.Username = HttpContext.Session.GetString("Username");
                ViewBag.UserImg = HttpContext.Session.GetString("UserImage");
            }
            var offer = await _offerQueries.GetOfferReport(id);
            if (offer == null) return RedirectToAction("Error", "Home");
            return View(offer);
        }
        public async Task<IActionResult> user(string id)
        {
            bool isuerloggedin = _loggedUser.IsUserLoggedIn();
            ViewBag.IsUserLoggedIn = isuerloggedin;
            if (!isuerloggedin)
            {
                return RedirectToAction("Login", "User");
            }
            else
            {
                string myid = _loggedUser.GetUserId();
                if(myid == id) return RedirectToAction("Index", "Home");
                ViewBag.Username = HttpContext.Session.GetString("Username");
                ViewBag.UserImg = HttpContext.Session.GetString("UserImage");
            }
            var user = await _userQueries.GetUserById(id);
            if(user == null) return RedirectToAction("Error", "Home");
            return View(user);
        }
        [HttpPost]
        public async Task<IActionResult> ReportOffer(string offerid, string reasonid)
        {
            bool isuerloggedin = _loggedUser.IsUserLoggedIn();
            if (!isuerloggedin) return RedirectToAction("Login", "User");
            string userid = _loggedUser.GetUserId();
            if (userid == null || userid == "") return RedirectToAction("Error", "Home");
            long offer_id = (long)Convert.ToDouble(offerid);
            long reason_id = (long)Convert.ToDouble(reasonid);
            if(await _reportQueries.DoesOfferReportExist(offer_id, userid))
            {
                return Json(new { success = false, text = "Ta oferta została przez ciebie zgłoszona!" });
            }
            else
            {
                await _reportQueries.ReportOffer(offer_id, userid, reason_id);
                return Json(new { success = true, text = "Oferta została zgłoszona!" });
            }
        }
        [HttpPost]
        public async Task<IActionResult> ReportUser(string userid, string reasonid)
        {
            bool isuerloggedin = _loggedUser.IsUserLoggedIn();
            if (!isuerloggedin) return RedirectToAction("Login", "User");
            string myid = _loggedUser.GetUserId();
            if (userid == null || userid == "") return RedirectToAction("Error", "Home");
            if (await _reportQueries.DoesUserReportExist(userid, myid))
            {
                return Json(new { success = false, text = "Ten użytkownik został przez ciebie zgłoszony!" });
            }
            else
            {
                long reason_id = (long)Convert.ToDouble(reasonid);
                await _reportQueries.ReportUser(userid, myid, reason_id);
                return Json(new { success = true, text = "Użytkownik został zgłoszony!" });
            }
        }
    }
}
