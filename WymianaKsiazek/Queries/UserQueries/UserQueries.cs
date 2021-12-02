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
using WymianaKsiazek.Queries.EmailQueries;

namespace WymianaKsiazek.Queries.UserQueries
{
    public class UserQueries : IUserQueries
    {
        private readonly IMapper _mapper;
        private readonly Context _context;
        private readonly UserManager<UserEntity> _userManager;
        private readonly SignInManager<UserEntity> _signInManager;
        private readonly IConfiguration _configuration;
        private readonly IEmailQueries _emailQueries;
        public UserQueries(IMapper mapper, Context context, UserManager<UserEntity> userManager,
            SignInManager<UserEntity> signInManager, IConfiguration configuration, IEmailQueries emailQueries)
        {
            _mapper = mapper;
            _context = context;
            _userManager = userManager;
            _signInManager = signInManager;
            _configuration = configuration;
            _emailQueries = emailQueries;
        }
        public async Task<UserEntity> GetUserEntity(string userid)
        {
            var user = await _userManager.FindByIdAsync(userid);
            return user;
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
                    await _emailQueries.SendEmailVerification(user, token);
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
                IsUserActiveInChat = false,
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
            token.Img = user.Img;
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
        public async Task<MyProfileMP> MyProfile(string userid)
        {
            var userdb = await _context.User.Include(x => x.Offers).ThenInclude(x => x.Book).Include(x => x.Offers).ThenInclude(x => x.User)
                .Include(x => x.Offers).ThenInclude(x => x.Address).Where(x => x.Id == userid).FirstOrDefaultAsync();
            return _mapper.Map<MyProfileMP>(userdb);
        }
        public async Task SendEmailResetPassword(UserEntity user)
        {
            var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            await _emailQueries.SendEmailResetPassword(user, token);
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
        public async Task<UserProfile> GetUserProfile(string id)
        {
            var user = await _context.User.Include(x => x.UserOpinion).Include(x => x.Offers).ThenInclude(x => x.Book).
                Include(x => x.Offers).ThenInclude(x => x.Address).Where(x => x.Id == id).FirstOrDefaultAsync();
            return _mapper.Map<UserProfile>(user);
        }
        public async Task<List<OffersListMP>> GetUsersLikedOffers(string userid)
        {
            var offers = await _context.UserLikedOffers.Include(x => x.Offer).ThenInclude(x => x.Book).Include(x => x.Offer)
                .ThenInclude(x => x.User).Include(x => x.Offer).ThenInclude(x => x.Address)
                .Where(x => x.User_Id == userid).Select(x => x.Offer).ToListAsync();
            return _mapper.Map<List<OffersListMP>>(offers);
        }
    }
}
