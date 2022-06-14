using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WymianaKsiazek.Functions;
using WymianaKsiazek.Queries.MessageQueries;
using WymianaKsiazek.Queries.UserQueries;

namespace WymianaKsiazek.Controllers
{
    [Authorize]
    public class MessageController : Controller
    {
        private readonly ILogger<MessageController> _logger;
        private readonly IMessageQueries _messageQueries;
        private readonly IUserQueries _userQueries;
        private readonly ILoggedInUser _loggedUser;
        public MessageController(ILogger<MessageController> logger, IMessageQueries messageQueries, IUserQueries userQueries,
            ILoggedInUser loggedUser)
        {
            _logger = logger;
            _messageQueries = messageQueries;
            _userQueries = userQueries;
            _loggedUser = loggedUser;
        }
        public async Task<IActionResult> Mymessages()
        {
            bool ifuserloggedin = _loggedUser.IsUserLoggedIn();
            if (!ifuserloggedin) return RedirectToAction("Login", "User");
            else
            {
                ViewBag.Username = HttpContext.Session.GetString("Username");
                ViewBag.UserImg = HttpContext.Session.GetString("UserImage");
            }
            string userid = _loggedUser.GetUserId();
            if (userid == null || userid == "") return RedirectToAction("Error", "Home", new { statuscode = 401 });
            var conversations = await _messageQueries.GetUserConversations(userid);
            ViewBag.MyId = userid;
            return View(conversations);
        }
        public async Task<IActionResult> Chat(long id)
        {
            bool isuserloggedin = _loggedUser.IsUserLoggedIn();
            if (!isuserloggedin) return RedirectToAction("Login", "User");
            else
            {
                ViewBag.Username = HttpContext.Session.GetString("Username");
                ViewBag.UserImg = HttpContext.Session.GetString("UserImage");
            }
            string myid = _loggedUser.GetUserId();
            if (myid == null || myid == "") return RedirectToAction("Error", "Home", new { statuscode = 401 });
            var conversation = await _messageQueries.GetConversationById(id, myid);
            if (conversation == null) return RedirectToAction("Error", "Home", new { statuscode = 404 });
            if (myid != conversation.User1.Id && myid != conversation.User2.Id) return RedirectToAction("Error", "Home", new { statuscode = 400 });
            conversation.Messages = (from i in conversation.Messages orderby i.SendDate select i).ToList();
            var otheruser = (myid != conversation.User1.Id) ? conversation.User1 : conversation.User2;
            var connectionid = await _messageQueries.GetUserConnectionId(otheruser.Id);
            await _messageQueries.ChangeUserStatus(myid, true);
            ViewBag.OtherUserId = otheruser;
            ViewBag.ConnectionId = connectionid;
            ViewBag.ConvId = id;
            ViewBag.MyId = myid;
            ViewBag.MyUserName = HttpContext.Session.GetString("Username");
            return View(conversation);
        }
        public async Task<IActionResult> LookForConversation(string userid)
        {
            bool isuserloggedin = _loggedUser.IsUserLoggedIn();
            if (!isuserloggedin) return RedirectToAction("Login", "User");
            string myid = _loggedUser.GetUserId();
            if (myid == null || myid == "") return RedirectToAction("Error", "Home", new { statuscode = 401 });
            if (myid == userid) return RedirectToAction("Index", "Home");
            var conv = await _messageQueries.CheckIfConversationExist(myid, userid);
            if (conv == null) return RedirectToAction("Startconversation", "Message", new { userid = userid });
            else return RedirectToAction("Chat", "Message", new { id = conv.Id });
        }
        public async Task<IActionResult> Startconversation(string userid)
        {
            bool isuserloggedin = _loggedUser.IsUserLoggedIn();
            if (!isuserloggedin) return RedirectToAction("Login", "User");
            else
            {
                ViewBag.Username = HttpContext.Session.GetString("Username");
                ViewBag.UserImg = HttpContext.Session.GetString("UserImage");
            }
            string myid = _loggedUser.GetUserId();
            if (myid == null || myid == "") return RedirectToAction("Error", "Home", new { statuscode = 401 });
            if (myid == userid) return RedirectToAction("Index", "Home");
            var user = await _userQueries.GetUserById(userid);
            if (user == null) return RedirectToAction("Error", "Home", new { statuscode = 400 });
            return View(user);
        }
        [HttpPost]
        public async Task<IActionResult> SendMessage(string messageInput, string convid, string otheruserid)
        {
            if (messageInput == null || messageInput == "") return Json(new { success = false, responseText = "Pusta wiadomość!" });
            if (!_loggedUser.IsUserLoggedIn()) return Json(new { success = false, responseText = "Unauthorized!" });
            string userid = _loggedUser.GetUserId();
            if (userid == null || userid == "") return Json(new { success = false, responseText = "Błąd uwierzytelnienia" });
            long conv_id = (long)Convert.ToDouble(convid);
            await _messageQueries.CreateMessage(conv_id, messageInput, userid, otheruserid);
            return Json(new { success = true });
        }
        [HttpPost]
        public async Task<IActionResult> UpdateChatConnectionId(string id)
        {
            string userid = _loggedUser.GetUserId();
            if (userid == null || userid == "") return Json(new { success = false, responseText = "Błąd autoryzacji!" });
            await _messageQueries.AddUserConnectionId(userid, id);
            return Json(new { success = true });
        }
        [HttpGet]
        public async Task<IActionResult> GetUserConnection(string userid)
        {
            var connectionid = await _messageQueries.GetUserConnectionId(userid);
            return Json(new { success = true, connectionId = connectionid });
        }
        [HttpPost]
        public async Task<IActionResult> UpdateUserStatus(string userid)
        {
            await _messageQueries.ChangeUserStatus(userid, false);
            return Json(new { success = true });
        }
        [HttpPost]
        public async Task<IActionResult> CreateNewConversation(string text, string userid)
        {
            bool isuserloggedin = _loggedUser.IsUserLoggedIn();
            if (!isuserloggedin) return RedirectToAction("Login", "User");
            string myid = _loggedUser.GetUserId();
            if (myid == null || myid == "") return RedirectToAction("Error", "Home", new { statuscode = 401 });
            await _messageQueries.CreateConversation(myid, userid);
            var conv = await _messageQueries.CheckIfConversationExist(myid, userid);
            if (conv == null) return RedirectToAction("Error", "Home", new { statuscode = 500 });
            await _messageQueries.CreateFirstMessage(conv.Id, myid, text);
            return RedirectToAction("Chat", "Message", new { id = conv.Id });
        }
    }
}
