using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Policy;
using System.Threading.Tasks;
using System.IdentityModel.Tokens.Jwt;
using WymianaKsiazek.Database;
using WymianaKsiazek.Database.Entities;
using WymianaKsiazek.Models.EmailModels;
using WymianaKsiazek.Models.MapperModels;
using WymianaKsiazek.Models;
using System.Text;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace WymianaKsiazek.Queries.UserQueries
{
    public class UserQueries : IUserQueries
    {
        private readonly IMapper _mapper;
        private readonly Context _context;
        private readonly IMailService _mailService;
        private readonly UserManager<UserEntity> _userManager;
        private readonly SignInManager<UserEntity> _signInManager;
        private readonly IConfiguration _configuration;
        private readonly IUrlHelper _urlHelper;
        private HttpRequest _request;
        public UserQueries(IMapper mapper, Context context, UserManager<UserEntity> userManager,
            SignInManager<UserEntity> signInManager, IConfiguration configuration, IMailService mailService, IUrlHelper urlHelper, IHttpContextAccessor httpContextAccessor)
        {
            _mapper = mapper;
            _context = context;
            _userManager = userManager;
            _signInManager = signInManager;
            _configuration = configuration;
            _mailService = mailService;
            _urlHelper = urlHelper;
            _request = httpContextAccessor.HttpContext.Request;
        }
        public async Task<UserMP> GetUserById(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            return _mapper.Map<UserMP>(user);
        }
        public async Task<int> CreateNewUser(CreateUserMP model)
        {
            using (var transaction = _context.Database.BeginTransaction())
            {
                var existingUser = await _userManager.FindByEmailAsync(model.Email);
                if (existingUser != null)
                {
                    return 1;
                }
                var existingUserByNickname = await _userManager.FindByNameAsync(model.UserName);
                if (existingUserByNickname != null)
                {
                    return 2;
                }
                UserEntity user = new UserEntity();
                user.Email = model.Email;
                user.UserName = model.UserName;
                long addressid = 0;
                addressid = _context.Address.Where(x => x.Name == model.AddressName).Select(x => x.Address_Id).FirstOrDefault();
                if (addressid == 0)
                {
                    return 4;
                }
                user.Address_Id = addressid;

                var passwordHash = new PasswordHasher<UserEntity>();
                var hashed = passwordHash.HashPassword(user, model.Password);

                user.PasswordHash = hashed;
                user.NormalizedEmail = user.Email.ToUpper();
                user.NormalizedUserName = user.UserName.ToUpper();
                user.SecurityStamp = Guid.NewGuid().ToString();

                var createResult = await _userManager.CreateAsync(user);
                if (!createResult.Succeeded)
                {
                    return 3;
                }
                else
                {
                    var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                    var confirmationlink = _urlHelper.Action("ConfirmEmail", "User", new { userId = user.Id, token = token}, _request.Scheme);
                    MailRequest mailRequest = new MailRequest();
                    mailRequest.ToEmail = model.Email;
                    mailRequest.Subject = "wymienksiazke.com - Weryfikacja konta";
                    string mailtext = "<div style = 'background-color: #696969; color: white; margin-left: auto; margin-right: auto; width: 100%;'>" +
                    "<div style = 'padding-top: 30px; text-align: center;'>" +
                        "Witaj, Twoje konto jest prawie gotowe!" +
                    "</div>" +
                    "<div style = 'padding-top: 30px; text-align: center;'>" +
                        "Kliknij poniższy przycisk aby aktywować swoje konto" +
                    "</div>" +
                    "<div style = 'background-color: #228B22; padding: 10px; width: 100px; margin-left: 42%; margin-top: 30px;'>" +
                        "<a href = '" + confirmationlink + "' style = 'text-decoration:none'>" +
                            "<div style = 'text-align: center; color: white;'>" +
                                "Weryfikuj" +
                            "</div>" +
                        "</a>" +
                    "</div>" +
                    "<div style = 'background-color: black; text-align: center; margin-top: 20px; color: white;'>" +
                        "wymienksiazke.com &copy Wszelkie prawa zastrzeżone!" +
                    "</div>" +
                    "</div>";
                    mailRequest.Body = mailtext;
                    await _mailService.SendEmailAsync(mailRequest);
                }
                transaction.Commit();
            }

            return 0;
        }
        public (string, DateTime) CreateAccessToken(UserEntity user)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id),
                new Claim(ClaimTypes.Email, user.Email)
            };

            var expires = DateTime.UtcNow.AddMinutes(15);

            var jwtSecurityToken = new JwtSecurityToken(
                _configuration["Jwt:Issuer"],
                _configuration["Jwt:Issuer"],
                claims,
                expires: expires,
                signingCredentials: credentials);

            var jwtSecurityTokenHandler = new JwtSecurityTokenHandler();

            return (jwtSecurityTokenHandler.WriteToken(jwtSecurityToken), expires);
        }
        public async Task<string> CreateRefreshToken(string userId)
        {
            var createdOn = DateTime.UtcNow;
            var expiresOn = createdOn.AddMonths(1);

            var refreshToken = new RefreshTokenEntity
            {
                UserId = userId,
                CreatedOn = createdOn,
                ExpiresOn = expiresOn,
                Token = Guid.NewGuid().ToString()
            };

            _context.RefreshTokens.Add(refreshToken);
            await _context.SaveChangesAsync();

            return refreshToken.Token;
        }
        public async Task<TokenOutput> CreateToken(string email, string password)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
            {
                return null;
            }
            var signInResult = await _signInManager.CheckPasswordSignInAsync(user, password, false);
            if (!signInResult.Succeeded)
            {
                return null;
            }
            var accessToken = CreateAccessToken(user);
            TokenOutput token = new TokenOutput();
            token.AccessToken = accessToken.Item1;
            token.Expires = accessToken.Item2;
            token.RefreshToken = await CreateRefreshToken(user.Id);
            token.Id = user.Id;
            token.Email = user.UserName;
            return token;
        }
        public async Task<int> RemoveRefreshToken(string token)
        {
            using (var transaction = _context.Database.BeginTransaction())
            {
                var refreshToken = await _context.RefreshTokens
                    .FirstOrDefaultAsync(x => x.Token == token);
                if (refreshToken == null)
                {
                    return 1;
                }

                _context.RefreshTokens.Remove(refreshToken);

                await _context.SaveChangesAsync();
                transaction.Commit();
            }
            return 0;
        }
        public MyProfileMP MyProfile(string userid)
        {
            var userdb = _context.User.Include(x => x.Offers).ThenInclude(x => x.Book).Include(x => x.Offers).ThenInclude(x => x.User)
                .Include(x => x.Offers).ThenInclude(x => x.Address).Where(x => x.Id == userid).FirstOrDefault();
            return _mapper.Map<MyProfileMP>(userdb);
        }
        public async Task SendEmailResetPassword(UserEntity user)
        {
            var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            var confirmationlink = _urlHelper.Action("ResetPassword", "User", new { userId = user.Id, token = token }, _request.Scheme);
            MailRequest mailRequest = new MailRequest();
            mailRequest.ToEmail = user.Email;
            mailRequest.Subject = "wymienksiazke.com - Reset hasła - Weryfikacja";
            string mailtext = "<div style = 'background-color: #696969; color: white; margin-left: auto; margin-right: auto; width: 100%;'>" +
            "<div style = 'padding-top: 30px; text-align: center;'>" +
                "Reset hasła - Weryfikacja!" +
            "</div>" +
            "<div style = 'padding-top: 30px; text-align: center;'>" +
                "Kliknij poniższy przycisk zresetować hasło" +
            "</div>" +
            "<div style = 'background-color: #228B22; padding: 10px; width: 100px; margin-left: 42%; margin-top: 30px;'>" +
                "<a href = '" + confirmationlink + "' style = 'text-decoration:none'>" +
                    "<div style = 'text-align: center; color: white;'>" +
                        "Resetuj" +
                    "</div>" +
                "</a>" +
            "</div>" +
            "<div style = 'background-color: black; text-align: center; margin-top: 20px; color: white;'>" +
                "wymienksiazke.com &copy Wszelkie prawa zastrzeżone!" +
            "</div>" +
            "</div>";
            mailRequest.Body = mailtext;
            await _mailService.SendEmailAsync(mailRequest);
        }
        public async Task<int> ChangeCurrentPassword(string userid, string oldpasswd, string newpasswd)
        {
            var user = await _userManager.FindByIdAsync(userid);
            if(user == null)
            {
                return 1;
            }
            var signInResult = await _signInManager.CheckPasswordSignInAsync(user, oldpasswd, false);
            if (!signInResult.Succeeded)
            {
                return 2;
            }
            await ChangePassword(user, newpasswd);
            return 0;
        }
        public async Task ChangePassword(UserEntity user, string password)
        {
            var passwordHash = new PasswordHasher<UserEntity>();
            var hashed = passwordHash.HashPassword(user, password);
            user.PasswordHash = hashed;
            using (var transaction = _context.Database.BeginTransaction())
            {
                _context.Update(user);
                await _context.SaveChangesAsync();
                transaction.Commit();
            }
        }
        public UserProfile GetUserProfile(string id)
        {
            var user = _context.User.Include(x => x.UserOpinion).Include(x => x.Offers).ThenInclude(x => x.Book).
                Include(x => x.Offers).ThenInclude(x => x.Address).Where(x => x.Id == id).FirstOrDefault();
            return _mapper.Map<UserProfile>(user);
        }
        public int GetUserOpinionAboutUser(string userprofileid, string usersopinionid)
        {
            var opinionid = _context.UserOpinions.Where(x => x.User_Id == userprofileid).Select(x => x.Opinion_Id).FirstOrDefault();
            if(opinionid == 0)
            {
                return -1;
            }
            var useropinion = 0;
            useropinion = _context.UserOpinionInfo.Where(x => x.Opinion_Id == opinionid && x.User_Id == usersopinionid).Select(x => x.Value).FirstOrDefault();
            return useropinion;
        }
        public async Task AddUserOpinionAboutUser(int value, long opinion_id, string userid)
        {
            UserUserOpinionEntity useropinion = new UserUserOpinionEntity();
            useropinion.User_Id = userid;
            useropinion.Opinion_Id = opinion_id;
            useropinion.Value = value;
            using(var transaction = _context.Database.BeginTransaction())
            {
                _context.Add(useropinion);
                var useropiniontable = _context.UserOpinions.Where(x => x.Opinion_Id == opinion_id).FirstOrDefault();
                if(useropiniontable != null)
                {
                    useropiniontable.OpinionSumValue += (uint)value;
                    useropiniontable.TotalOpinions += 1;
                }
                await _context.SaveChangesAsync();
                transaction.Commit();
            }
        }
        public async Task LikeOffer(string userid, long offerid)
        {
            UserLikedOffersEntity like = new UserLikedOffersEntity();
            like.User_Id = userid;
            like.Offer_Id = offerid;
            using(var transaction = _context.Database.BeginTransaction())
            {
                _context.Add(like);
                await _context.SaveChangesAsync();
                transaction.Commit();
            }
        }
        public async Task UnLikeOffer(string userid, long offerid)
        {
            var like = _context.UserLikedOffers.Where(x => x.User_Id == userid && x.Offer_Id == offerid).FirstOrDefault();
			if(like != null)
			{
				_context.UserLikedOffers.Attach(like);
				_context.UserLikedOffers.Remove(like);
				await _context.SaveChangesAsync();
			}
        }
        public List<OffersListMP> GetUsersLikedOffers(string userid)
        {
            var offers = _context.UserLikedOffers.Include(x => x.Offer).ThenInclude(x => x.Book).Include(x => x.Offer).ThenInclude(x => x.User)
                .Include(x => x.Offer).ThenInclude(x => x.Address).Where(x => x.User_Id == userid).Select(x => x.Offer).ToList();
            return _mapper.Map<List<OffersListMP>>(offers);
        }
        public ConversationMP GetConversationById(long id)
        {
            var conversation = _context.Conversations.Include(x => x.User1).Include(x => x.User2).Include(x => x.Messages)
                .Where(x => x.Id == id).FirstOrDefault();
            return _mapper.Map<ConversationMP>(conversation);
        }
        public async Task CreateMessage(long convid, string text, string userid)
        {
            MessageEntity message = new MessageEntity();
            using (var transaction = _context.Database.BeginTransaction())
            {
                message.Text = text;
                message.Conv_Id = convid;
                message.User_Id = userid;
                await _context.AddAsync(message);
                await _context.SaveChangesAsync();
            }
        }
    }
}
