using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WymianaKsiazek.Database.Entities;
using WymianaKsiazek.Functions;
using WymianaKsiazek.Queries.EditQueries;
using WymianaKsiazek.Queries.EmailQueries;
using WymianaKsiazek.Queries.UserQueries;

namespace WymianaKsiazek.Controllers
{
    [Authorize]
    public class EditController : Controller
    {
        private readonly IEditQueries _editQueries;
        private readonly IUserQueries _userQueries;
        private readonly ILoggedInUser _loggedInUser;
        private readonly IEmailQueries _emailQueries;
        private readonly ISaveImg _saveImg;
        private readonly UserManager<UserEntity> _userManager;
        public EditController(IEditQueries editQueries, IUserQueries userQueries, ILoggedInUser loggedInUser, 
            IEmailQueries emailQueries, ISaveImg saveImg, UserManager<UserEntity> userManager)
        {
            _editQueries = editQueries;
            _userQueries = userQueries;
            _loggedInUser = loggedInUser;
            _emailQueries = emailQueries;
            _saveImg = saveImg;
            _userManager = userManager;
        }
        public async Task<IActionResult> Email(string oldemail, string newemail)
        {
            string userid = _loggedInUser.GetUserId();
            if (userid == null || userid == "") return RedirectToAction("Error", "Home", new { statuscode = 401 });
            var user = await _userQueries.GetUserEntity(userid);
            if (user == null) return RedirectToAction("Error", "Home", new { statuscode = 401 });
            if (user.Email != oldemail)
            {
                TempData["EditError"] = "Email nieprawidłowy!";
                return RedirectToAction("EditProfile", "User");
            }
            var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            await _emailQueries.SendEmailChange(user, token, newemail);
            return RedirectToAction("Confirmation", "Edit", new { email = newemail });
        }
        public IActionResult Confirmation(string email)
        {
            bool isuerloggedin = _loggedInUser.IsUserLoggedIn();
            ViewBag.IsUserLoggedIn = isuerloggedin;
            if (!isuerloggedin)
            {
                return RedirectToAction("Login", "User");
            }
            else
            {
                ViewBag.Username = HttpContext.Session.GetString("Username");
                ViewBag.UserImg = HttpContext.Session.GetString("UserImage");
                ViewBag.Email = email;
            }
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> ChangeEmail(string newemail, string token, string userid)
        {
            bool isuerloggedin = _loggedInUser.IsUserLoggedIn();
            if (!isuerloggedin) return RedirectToAction("Index", "Home");
            string id = _loggedInUser.GetUserId();
            if (token == null || userid == null) return RedirectToAction("Error", "Home", new { statuscode = 401 });
            var user = await _userManager.FindByIdAsync(userid);
            if (user.Id != id) return RedirectToAction("Error", "Home", new { statuscode = 401 });
            await _editQueries.ChangeEmail(user, newemail);
            TempData["ChangeInfo"] = "Email zmieniony pomyślnie!";
            return RedirectToAction("MyProfile", "User");
        }
        [HttpPost]
        public async Task<IActionResult> ChangeUserName(string oldusername, string newusername)
        {
            bool isuerloggedin = _loggedInUser.IsUserLoggedIn();
            if (!isuerloggedin) return RedirectToAction("Login", "User");
            var user = await _userManager.FindByIdAsync(_loggedInUser.GetUserId());
            if (user == null) return RedirectToAction("Error", "Home", new { statuscode = 401 });
            if (user.UserName != oldusername)
            {
                TempData["EditError"] = "Nazwa użytkownika nieprawidłowa!";
                    return RedirectToAction("EditProfile", "User");
            }
            await _editQueries.ChangeUsername(user, newusername);
            TempData["ChangeInfo"] = "Nazwa użytkownika zmieniona pomyślnie!";
            HttpContext.Session.SetString("Username", newusername);
            return RedirectToAction("MyProfile", "User");
        }
        [HttpPost]
        public async Task<IActionResult> ChangeImage(IFormFile newimg)
        {
            bool isuerloggedin = _loggedInUser.IsUserLoggedIn();
            if (!isuerloggedin) return RedirectToAction("Login", "User");
            var user = await _userManager.FindByIdAsync(_loggedInUser.GetUserId());
            if (user == null) return RedirectToAction("Error", "Home", new { statuscode = 401 });
            string imagename = _saveImg.SaveImage(newimg);
            if (imagename == null) return RedirectToAction("Error", "Home", new { statuscode = 400 });
            await _editQueries.ChangeImage(user, imagename);
            TempData["ChangeInfo"] = "Zdjęcie zmieniono pomyślnie!";
            HttpContext.Session.SetString("UserImage", imagename);
            return RedirectToAction("MyProfile", "User");
        }
    }
}
