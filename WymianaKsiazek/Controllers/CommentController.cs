using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WymianaKsiazek.Functions;
using WymianaKsiazek.Queries.CommentQueries;

namespace WymianaKsiazek.Controllers
{
    public class CommentController : Controller
    {
        private readonly ICommentQueries _commentQueries;
        private readonly ILoggedInUser _loggedUser;
        public CommentController(ICommentQueries commentQueries, ILoggedInUser loggedUser)
        {
            _commentQueries = commentQueries;
            _loggedUser = loggedUser;
        }
        [HttpPost]
        public async Task<IActionResult> AddComment(string addcommentinput, string offerid)
        {
            if (!_loggedUser.IsUserLoggedIn()) return RedirectToAction("Login", "User");
            string myuserid = _loggedUser.GetUserId();
            if (myuserid == null || myuserid == "") return RedirectToAction("Error", "Home");
            string username = HttpContext.Session.GetString("Username");
            await _commentQueries.AddCommentToOffer(addcommentinput, myuserid, (long)Convert.ToDouble(offerid));
            return Json(new { success = true, username = username });
        }
        [HttpPost]
        public async Task<IActionResult> AddCommentBook(string addcommentinput, string bookid)
        {
            if (!_loggedUser.IsUserLoggedIn()) return RedirectToAction("Login", "User");
            string myuserid = _loggedUser.GetUserId();
            if (myuserid == null || myuserid == "") return RedirectToAction("Error", "Home");
            string username = HttpContext.Session.GetString("Username");
            await _commentQueries.AddCommentToBook(addcommentinput, myuserid, (long)Convert.ToDouble(bookid));
            return Json(new { success = true, username = username });
        }
    }
}
