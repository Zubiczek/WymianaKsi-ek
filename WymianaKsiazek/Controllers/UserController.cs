using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using System.Diagnostics;
using WymianaKsiazek.Models.MapperModels;
using WymianaKsiazek.Database;
using Microsoft.AspNetCore.Identity;
using WymianaKsiazek.Database.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
using System.Security.Claims;
using System.Text;
using AutoMapper;
using WymianaKsiazek.Models;
using Microsoft.AspNetCore.Http;
using WymianaKsiazek.Queries.UserQueries;
using WymianaKsiazek.Models.EmailModels;
using System.Dynamic;

namespace WymianaKsiazek.Controllers
{
    public class UserController : Controller
    {
        private readonly ILogger<UserController> _logger;
        private readonly Context _context;
        private readonly UserManager<UserEntity> _userManager;
        private readonly SignInManager<UserEntity> _signInManager;
        private readonly IConfiguration _configuration;
        private readonly IUserQueries _userQueries;
        private readonly IMailService _mailService;
        public UserController(ILogger<UserController> logger, Context context, UserManager<UserEntity> userManager, 
            SignInManager<UserEntity> signInManager, IConfiguration configuration, IUserQueries userQueries, IMailService mailService)
        {
            _logger = logger;
            _context = context;
            _userManager = userManager;
            _signInManager = signInManager;
            _configuration = configuration;
            _userQueries = userQueries;
            _mailService = mailService;
        }
        public IActionResult Register()
        {
            bool isuerloggedin = IsUserLoggedIn();
            ViewBag.IsUserLoggedIn = isuerloggedin;
            if (isuerloggedin == true)
            {
                return RedirectToAction("Index", "Home");
            }
            ViewBag.ExistingUserError = TempData["existingusererror"];
            TempData["existingusererror"] = null;
            return View();
        }
        public IActionResult Login()
        {
            bool isuerloggedin = IsUserLoggedIn();
            ViewBag.IsUserLoggedIn = isuerloggedin;
            if (isuerloggedin == true)
            {
                return RedirectToAction("Index", "Home");
            }
            ViewBag.LoginError = TempData["LoginError"];
            TempData["LoginError"] = null;
            return View();
        }
        public IActionResult EmailConfirmationPage(string email)
        {
            bool isuerloggedin = IsUserLoggedIn();
            ViewBag.IsUserLoggedIn = isuerloggedin;
            ViewBag.Email = email;
            return View();
        }
        public IActionResult EmailConfirmedPage()
        {
            bool isuerloggedin = IsUserLoggedIn();
            ViewBag.IsUserLoggedIn = isuerloggedin;
            return View();
        }
        public IActionResult MyProfile()
        {
            bool isuerloggedin = IsUserLoggedIn();
            ViewBag.IsUserLoggedIn = isuerloggedin;
            if (isuerloggedin == false)
            {
                return RedirectToAction("Login", "User");
            }
            string userid = GetUserId();
            if (userid == null)
            {
                return RedirectToAction("Error", "Home");
            }
            var user = _userQueries.MyProfile(userid);
            if(user == null)
            {
                return RedirectToAction("Error", "Home");
            }
            ViewBag.ChangeInfo = TempData["ChangeInfo"];
            var userlikedoffers = _userQueries.GetUsersLikedOffers(userid);
            dynamic model = new ExpandoObject();
            model.Profile = user;
            model.LikedOffers = userlikedoffers;
            TempData["ChangeInfo"] = null;
            return View(model);
        }
        public IActionResult ForgotPassword()
        {
            bool isuerloggedin = IsUserLoggedIn();
            ViewBag.IsUserLoggedIn = isuerloggedin;
            if (isuerloggedin == true)
            {
                return RedirectToAction("Index", "Home");
            }
            ViewBag.Error = TempData["ForgotPasswordError"];
            TempData["ForgotPasswordError"] = null;
            return View();
        }
        public IActionResult ForgotPasswordInformation(string email)
        {
            bool isuerloggedin = IsUserLoggedIn();
            ViewBag.IsUserLoggedIn = isuerloggedin;
            ViewBag.Email = email;
            return View();
        }
        public IActionResult ResetPassword(string userId, string token)
        {
            bool isuerloggedin = IsUserLoggedIn();
            ViewBag.IsUserLoggedIn = isuerloggedin;
            if (userId == null || token == null)
            {
                return RedirectToAction("Error", "Home");
            }
            ViewBag.UserId = userId;
            return View();
        }
        public IActionResult ChangePassword()
        {
            bool isuerloggedin = IsUserLoggedIn();
            ViewBag.IsUserLoggedIn = isuerloggedin;
            if (IsUserLoggedIn() == false)
            {
                return RedirectToAction("Error", "Home");
            }
            ViewBag.Error = TempData["PasswordError"];
            TempData["PasswordError"] = null;
            return View();
        }
        public IActionResult Profile(string id)
        {
            var user = _userQueries.GetUserProfile(id);
            if(user == null)
            {
                return RedirectToAction("Error", "Home");
            }
            bool isuserloggedin = IsUserLoggedIn();
            ViewBag.IsUserLoggedIn = isuserloggedin;
            if(isuserloggedin == true)
            {
                string userid = GetUserId();
                if(userid == null)
                {
                    return RedirectToAction("Error", "Home");
                }
                ViewBag.UserOpinion = _userQueries.GetUserOpinionAboutUser(id, userid);
            }
            return View(user);
        }
        public async Task<IActionResult> Chat(long id)
        {
            bool isuserloggedin = IsUserLoggedIn();
            if (!isuserloggedin) return RedirectToAction("Login", "User");
            var user = await _userQueries.GetUserById(GetUserId());
            if (user == null) return RedirectToAction("Error", "Home");
            var conversation = _userQueries.GetConversationById(id);
            if(conversation == null) return RedirectToAction("Error", "Home");
            if(user.Id != conversation.User1.Id && user.Id != conversation.User2.Id) return RedirectToAction("Error", "Home");
            ViewBag.MyId = user.Id;
            return View(conversation);
        }
        public async Task<IActionResult> SendMessage(string text, string convid)
        {
            if(text == null || text == "") return BadRequest();
            if(!IsUserLoggedIn()) return BadRequest();
            string userid = GetUserId();
            if(userid == null || userid == "") return BadRequest();
            long conv_id = (long)Convert.ToDouble(convid);
            await _userQueries.CreateMessage(conv_id, text, userid);
            return Ok();
        }

        [HttpPost]
        public async Task<IActionResult> AddOpinionAboutUser(int value, string opinionid)
        {
            long opinion_id = (long)Convert.ToDouble(opinionid);
            if(IsUserLoggedIn() == false)
            {
                return Json(new { success = false, responseText = "Unauthorized!" });
            }
            else
            {
                string userid = GetUserId();
                if (userid == null || userid == "")
                {
                    return Json(new { success = false, responseText = "Server Error!" });
                }
                await _userQueries.AddUserOpinionAboutUser(value, opinion_id, userid);
                return Json(new { success = true });
            }
        }
        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> SignUpUser(string emailinput, string usernameinput, string cityinput, string passwordinput)
        {
            CreateUserMP user = new CreateUserMP();
            user.Email = emailinput;
            user.UserName = usernameinput;
            user.AddressName = cityinput;
            user.Password = passwordinput;
            int result = await _userQueries.CreateNewUser(user);
            if(result == 1)
            {
                TempData["existingusererror"] = "Konto o podanym adresie email już istnieje!";
                return RedirectToAction("Register", "User");
            }
            else if(result == 2)
            {
                TempData["existingusererror"] = "Nazwa użytkownika zajęta!";
                return RedirectToAction("Register", "User");
            }
            else if(result == 3)
            {
                return RedirectToAction("Error", "Home");
            }
            else if(result == 4)
            {
                TempData["existingusererror"] = "Podane miasto nie istnieje!";
                return RedirectToAction("Register", "User");
            }
            else
            {
                return RedirectToAction("EmailConfirmationPage", "User", new { email = emailinput});
            }
        }
        public async Task<IActionResult> ConfirmEmail(string userId, string token)
        {
            if (userId == null || token == null)
            {
                return RedirectToAction("Error", "Index");
            }
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return RedirectToAction("Error", "Index");
            }
            var result = await _userManager.ConfirmEmailAsync(user, token);
            if (!result.Succeeded)
            {
                return RedirectToAction("Error", "Index");
            }
            else
            {
                return RedirectToAction("EmailConfirmedPage", "User");
            }
        }
        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> LogIn(string email, string password)
        {
            var token = await _userQueries.CreateToken(email, password);
            if(token == null)
            {
                TempData["LoginError"] = "Email lub hasło nieprawidłowe!";
                return RedirectToAction("Login", "User");
            }
            HttpContext.Session.SetString("Token", token.AccessToken);
            HttpContext.Session.SetString("RefreshToken", token.RefreshToken);
            return RedirectToAction("Index", "Home");
        }
        public async Task<IActionResult> LogOut()
        {
            string token = HttpContext.Session.GetString("RefreshToken");
            if(token == null)
            {
                return RedirectToAction("Error", "Home");
            }
            int result = await _userQueries.RemoveRefreshToken(token);
            if(result == 1)
            {
                return RedirectToAction("Error", "Home");
            }
            else
            {
                HttpContext.Session.Remove("Token");
                HttpContext.Session.Remove("RefreshToken");
                return RedirectToAction("Index", "Home");
            }
        }
        public async Task<IActionResult> ForgotPasswordSendEmail(string emailinput, string usernameinput)
        {
            var user = await _userManager.FindByEmailAsync(emailinput);
            if(user == null)
            {
                TempData["ForgotPasswordError"] = "Nie istnieje konto o podanym adresie e-mail";
                return RedirectToAction("ForgotPassword", "User");
            }
            if(user.UserName != usernameinput)
            {
                TempData["ForgotPasswordError"] = "Nieprawidłowa nazwa użytkownika!";
                return RedirectToAction("ForgotPassword", "User");
            }
            await _userQueries.SendEmailResetPassword(user);
            return RedirectToAction("ForgotPasswordInformation", "User", new { email = emailinput});
        }
        public async Task<IActionResult> ResetChangePassword(string userid, string passwordinput)
        {
            if (userid == null || passwordinput == null)
            {
                return RedirectToAction("Error", "Home");
            }
            var user = await _userManager.FindByIdAsync(userid);
            if(user == null)
            {
                return RedirectToAction("Error", "Home");
            }
            await _userQueries.ChangePassword(user, passwordinput);
            return RedirectToAction("Login", "User");
        }
        public async Task<IActionResult> ChangeCurrentPassword(string oldpasswordinput, string passwordinput)
        {
            if(oldpasswordinput == null || passwordinput == null)
            {
                return RedirectToAction("Error", "Home");
            }
            string userid = GetUserId();
            if(userid == null)
            {
                return RedirectToAction("Error", "Home");
            }
            int result = await _userQueries.ChangeCurrentPassword(userid, oldpasswordinput, passwordinput);
            if(result == 1)
            {
                return RedirectToAction("Error", "Home");
            }
            else if(result == 2)
            {
                TempData["PasswordError"] = "Nieprawidłowe hasło!";
                return RedirectToAction("ChangePassword", "User");
            }
            TempData["ChangeInfo"] = "Hasło zmienione pomyślnie!";
            return RedirectToAction("MyProfile", "User");
        }
        private bool IsUserLoggedIn()
        {
            bool p = false;
            string token = HttpContext.Session.GetString("Token");
            if (token != null)
            {
                p = true;
            }
            return p;
        }
        private string GetUserId()
        {
            var refreshtoken = HttpContext.Session.GetString("RefreshToken");
            string userid = _context.RefreshTokens.Where(x => x.Token == refreshtoken).Select(x => x.UserId).FirstOrDefault();
            return userid;
        }
    }
}
