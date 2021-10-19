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
            ViewBag.IsUserLoggedIn = IsUserLoggedIn();
            if (IsUserLoggedIn() == true)
            {
                return RedirectToAction("Index", "Home");
            }
            ViewBag.ExistingUserError = TempData["existingusererror"];
            TempData["existingusererror"] = null;
            return View();
        }
        public IActionResult Login()
        {
            ViewBag.IsUserLoggedIn = IsUserLoggedIn();
            if (IsUserLoggedIn() == true)
            {
                return RedirectToAction("Index", "Home");
            }
            ViewBag.LoginError = TempData["LoginError"];
            TempData["LoginError"] = null;
            return View();
        }
        public IActionResult EmailConfirmationPage(string email)
        {
            ViewBag.IsUserLoggedIn = IsUserLoggedIn();
            ViewBag.Email = email;
            return View();
        }
        public IActionResult EmailConfirmedPage()
        {
            ViewBag.IsUserLoggedIn = IsUserLoggedIn();
            return View();
        }
        public IActionResult MyProfile()
        {
            ViewBag.IsUserLoggedIn = IsUserLoggedIn();
            if (IsUserLoggedIn() == false)
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
            return View(user);
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
