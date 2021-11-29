using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WymianaKsiazek.Functions;
using WymianaKsiazek.Queries.LikeQueries;

namespace WymianaKsiazek.Controllers
{
    public class FollowController : Controller
    {
        private readonly ILikeQueries _likeQueries;
        private readonly ILoggedInUser _loggedUser;
        public FollowController(ILikeQueries likeQueries, ILoggedInUser loggedUser)
        {
            _likeQueries = likeQueries;
            _loggedUser = loggedUser;
        }
        [HttpPost]
        public async Task<IActionResult> LikeOffer(long offerid)
        {
            if (!_loggedUser.IsUserLoggedIn())
            {
                return Json(new { success = false, responseText = "Unauthorized!" });
            }
            else
            {
                string userid = _loggedUser.GetUserId();
                if (userid == null || userid == "")
                {
                    return Json(new { success = false, responseText = "Server Error!" });
                }
                else
                {
                    await _likeQueries.LikeOffer(userid, offerid);
                    return Json(new { success = true });
                }
            }
        }
        [HttpPost]
        public async Task<IActionResult> UnLikeOffer(long offerid)
        {
            if (!_loggedUser.IsUserLoggedIn())
            {
                return Json(new { success = false, responseText = "Unauthorized!" });
            }
            else
            {
                string userid = _loggedUser.GetUserId();
                if (userid == null || userid == "")
                {
                    return Json(new { success = false, responseText = "Server Error!" });
                }
                else
                {
                    await _likeQueries.UnLikeOffer(userid, offerid);
                    return Json(new { success = true });
                }
            }
        }
    }
}
