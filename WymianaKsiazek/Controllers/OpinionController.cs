using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WymianaKsiazek.Functions;
using WymianaKsiazek.Queries.OpinionQueries;

namespace WymianaKsiazek.Controllers
{
    [Authorize]
    public class OpinionController : Controller
    {
        private readonly IOpinionQueries _opinionQueries;
        private readonly ILoggedInUser _loggedUser;
        public OpinionController(IOpinionQueries opinionQueries, ILoggedInUser loggedUser)
        {
            _opinionQueries = opinionQueries;
            _loggedUser = loggedUser;
        }
        [HttpPost]
        public async Task<IActionResult> AddOpinionAboutUser(int value, string opinionid)
        {
            long opinion_id = (long)Convert.ToDouble(opinionid);
            if (!_loggedUser.IsUserLoggedIn())  return Json(new { success = false, responseText = "Unauthorized!" });
            else
            {
                string userid = _loggedUser.GetUserId();
                if (userid == null || userid == "") return Json(new { success = false, responseText = "Server Error!" });
                await _opinionQueries.AddUserOpinionAboutUser(value, opinion_id, userid);
                return Json(new { success = true });
            }
        }
        [HttpPost]
        public async Task<IActionResult> AddOpinion(int value, string opinionid)
        {
            long opinion_id = (long)Convert.ToDouble(opinionid);
            if (!_loggedUser.IsUserLoggedIn())  return Json(new { success = false, responseText = "Unauthorized!" });
            else
            {
                string userid = _loggedUser.GetUserId();
                if (userid == null || userid == "") return Json(new { success = false, responseText = "Server Error!" });
                await _opinionQueries.AddUserOpinionAboutBook(value, opinion_id, userid);
                return Json(new { success = true });
            }
        }
    }
}
