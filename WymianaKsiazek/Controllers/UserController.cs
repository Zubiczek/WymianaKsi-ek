﻿using Microsoft.AspNetCore.Mvc;
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
using WymianaKsiazek.Queries.MessageQueries;
using WymianaKsiazek.Queries.OpinionQueries;
using WymianaKsiazek.Functions;

namespace WymianaKsiazek.Controllers
{
    [AllowAnonymous]
    public class UserController : Controller
    {
        private readonly ILogger<UserController> _logger;
        private readonly UserManager<UserEntity> _userManager;
        private readonly SignInManager<UserEntity> _signInManager;
        private readonly IConfiguration _configuration;
        private readonly IUserQueries _userQueries;
        private readonly IOpinionQueries _opinionQueries;
        private readonly ILoggedInUser _loggedUser;
        public UserController(ILogger<UserController> logger, UserManager<UserEntity> userManager, 
            SignInManager<UserEntity> signInManager, IConfiguration configuration, IUserQueries userQueries,
            IOpinionQueries opinionQueries, ILoggedInUser loggedUser)
        {
            _logger = logger;
            _userManager = userManager;
            _signInManager = signInManager;
            _configuration = configuration;
            _userQueries = userQueries;
            _opinionQueries = opinionQueries;
            _loggedUser = loggedUser;
        }
        public IActionResult Register()
        {
            bool isuerloggedin = _loggedUser.IsUserLoggedIn();
            ViewBag.IsUserLoggedIn = isuerloggedin;
            if (isuerloggedin)  return RedirectToAction("Index", "Home");
            ViewBag.ExistingUserError = TempData["existingusererror"];
            TempData["existingusererror"] = null;
            return View();
        }
        public IActionResult Login()
        {
            bool isuerloggedin = _loggedUser.IsUserLoggedIn();
            ViewBag.IsUserLoggedIn = isuerloggedin;
            if (isuerloggedin)  return RedirectToAction("Index", "Home");
            ViewBag.LoginError = TempData["LoginError"];
            TempData["LoginError"] = null;
            return View();
        }
        public IActionResult EmailConfirmationPage(string email)
        {
            bool isuerloggedin = _loggedUser.IsUserLoggedIn();
            ViewBag.IsUserLoggedIn = isuerloggedin;
            if (isuerloggedin)
            {
                ViewBag.Username = HttpContext.Session.GetString("Username");
                ViewBag.UserImg = HttpContext.Session.GetString("UserImage");
            }
            ViewBag.Email = email;
            return View();
        }
        public IActionResult EmailConfirmedPage()
        {
            bool isuerloggedin = _loggedUser.IsUserLoggedIn();
            ViewBag.IsUserLoggedIn = isuerloggedin;
            if (isuerloggedin)
            {
                ViewBag.Username = HttpContext.Session.GetString("Username");
                ViewBag.UserImg = HttpContext.Session.GetString("UserImage");
            }
            return View();
        }
        [Authorize]
        [HttpGet]
        public async Task<IActionResult> MyProfile()
        {
            bool isuerloggedin = _loggedUser.IsUserLoggedIn();
            ViewBag.IsUserLoggedIn = isuerloggedin;
            if (!isuerloggedin) return RedirectToAction("Login", "User");
            else
            {
                ViewBag.Username = HttpContext.Session.GetString("Username");
                ViewBag.UserImg = HttpContext.Session.GetString("UserImage");
            }
            string userid = _loggedUser.GetUserId();
            if (userid == null) return RedirectToAction("Error", "Home", new { statuscode = 401 });
            var user = await _userQueries.MyProfile(userid);
            if(user == null) return RedirectToAction("Error", "Home", new { statuscode = 404 });
            ViewBag.ChangeInfo = TempData["ChangeInfo"];
            var userlikedoffers = await _userQueries.GetUsersLikedOffers(userid);
            dynamic model = new ExpandoObject();
            model.Profile = user;
            model.LikedOffers = userlikedoffers;
            TempData["ChangeInfo"] = null;
            return View(model);
        }
        public IActionResult ForgotPassword()
        {
            bool isuerloggedin = _loggedUser.IsUserLoggedIn();
            ViewBag.IsUserLoggedIn = isuerloggedin;
            if (isuerloggedin) return RedirectToAction("Index", "Home");
            ViewBag.Error = TempData["ForgotPasswordError"];
            TempData["ForgotPasswordError"] = null;
            return View();
        }
        public IActionResult ForgotPasswordInformation(string email)
        {
            bool isuerloggedin = _loggedUser.IsUserLoggedIn();
            ViewBag.IsUserLoggedIn = isuerloggedin;
            if (isuerloggedin)
            {
                ViewBag.Username = HttpContext.Session.GetString("Username");
                ViewBag.UserImg = HttpContext.Session.GetString("UserImage");
            }
            ViewBag.Email = email;
            return View();
        }
        public IActionResult ResetPassword(string userId, string token)
        {
            bool isuerloggedin = _loggedUser.IsUserLoggedIn();
            ViewBag.IsUserLoggedIn = isuerloggedin;
            if (isuerloggedin)
            {
                ViewBag.Username = HttpContext.Session.GetString("Username");
                ViewBag.UserImg = HttpContext.Session.GetString("UserImage");
            }
            if (userId == null || token == null) return RedirectToAction("Error", "Home", new { statuscode = 401 });
            ViewBag.UserId = userId;
            return View();
        }
        [Authorize]
        public IActionResult ChangePassword()
        {
            bool isuerloggedin = _loggedUser.IsUserLoggedIn();
            ViewBag.IsUserLoggedIn = isuerloggedin;
            if (!isuerloggedin) return RedirectToAction("Login", "User");
            else
            {
                ViewBag.Username = HttpContext.Session.GetString("Username");
                ViewBag.UserImg = HttpContext.Session.GetString("UserImage");
            }
            ViewBag.Error = TempData["PasswordError"];
            TempData["PasswordError"] = null;
            return View();
        }
        public async Task<IActionResult> Profile(string id)
        {
            var user = await _userQueries.GetUserProfile(id);
            if(user == null) return RedirectToAction("Error", "Home", new { statuscode = 404 });
            bool isuserloggedin = _loggedUser.IsUserLoggedIn();
            ViewBag.IsUserLoggedIn = isuserloggedin;
            if(isuserloggedin)
            {
                ViewBag.Username = HttpContext.Session.GetString("Username");
                ViewBag.UserImg = HttpContext.Session.GetString("UserImage");
                string userid = _loggedUser.GetUserId();
                if(userid == null) return RedirectToAction("Error", "Home", new { statuscode = 401 });
                ViewBag.UserOpinion = await _opinionQueries.GetUserOpinionAboutUser(id, userid);
            }
            return View(user);
        }
        [Authorize]
        public IActionResult EditProfile()
        {
            bool isuerloggedin = _loggedUser.IsUserLoggedIn();
            ViewBag.IsUserLoggedIn = isuerloggedin;
            if (!isuerloggedin) return RedirectToAction("Login", "User");
            else
            {
                ViewBag.Username = HttpContext.Session.GetString("Username");
                ViewBag.UserImg = HttpContext.Session.GetString("UserImage");
                ViewBag.Error = TempData["EditError"];
            }
            return View();
        }
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
                return RedirectToAction("Error", "Home", new { statuscode = 500 });
            else if(result == 4)
            {
                TempData["existingusererror"] = "Podane miasto nie istnieje!";
                return RedirectToAction("Register", "User");
            }
            else
                return RedirectToAction("EmailConfirmationPage", "User", new { email = emailinput});
        }
        public async Task<IActionResult> ConfirmEmail(string userId, string token)
        {
            if (userId == null || token == null) return RedirectToAction("Error", "Home", new { statuscode = 401 });
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null) return RedirectToAction("Error", "Home", new { statuscode = 401 });
            var result = await _userManager.ConfirmEmailAsync(user, token);
            if (!result.Succeeded) return RedirectToAction("Error", "Home", new { statuscode = 500 });
            else return RedirectToAction("EmailConfirmedPage", "Home");
        }
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
            HttpContext.Session.SetString("Username", token.Email);
            HttpContext.Session.SetString("UserImage", token.Img);
            HttpContext.Session.SetString("UserId", token.Id);
            return RedirectToAction("Index", "Home");
        }
        [Authorize]
        public async Task<IActionResult> LogOut()
        {
            string token = HttpContext.Session.GetString("RefreshToken");
            if(token == null) return RedirectToAction("Error", "Home", new { statuscode = 401 });
            int result = await _userQueries.RemoveRefreshToken(token);
            if(result == 1) return RedirectToAction("Error", "Home", new { statuscode = 400 });
            else
            {
                HttpContext.Session.Remove("Token");
                HttpContext.Session.Remove("RefreshToken");
                HttpContext.Session.Remove("Username");
                HttpContext.Session.Remove("UserImage");
                HttpContext.Session.Remove("UserId");
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
            if (userid == null || passwordinput == null) return RedirectToAction("Error", "Home", new { statuscode = 400 });
            var user = await _userManager.FindByIdAsync(userid);
            if(user == null) return RedirectToAction("Error", "Home", new { statuscode = 401 });
            await _userQueries.ChangePassword(user, passwordinput);
            return RedirectToAction("Login", "User");
        }
        [Authorize]
        public async Task<IActionResult> ChangeCurrentPassword(string oldpasswordinput, string passwordinput)
        {
            if(oldpasswordinput == null || passwordinput == null) return RedirectToAction("Error", "Home", new { statuscode = 400 });
            string userid = _loggedUser.GetUserId();
            if(userid == null) return RedirectToAction("Error", "Home", new { statuscode = 401 });
            int result = await _userQueries.ChangeCurrentPassword(userid, oldpasswordinput, passwordinput);
            if(result == 1) return RedirectToAction("Error", "Home", new { statuscode = 500 });
            else if(result == 2)
            {
                TempData["PasswordError"] = "Nieprawidłowe hasło!";
                return RedirectToAction("ChangePassword", "User");
            }
            TempData["ChangeInfo"] = "Hasło zmienione pomyślnie!";
            return RedirectToAction("MyProfile", "User");
        }
    }
}
