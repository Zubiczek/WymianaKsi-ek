using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WymianaKsiazek.Database.Entities;
using WymianaKsiazek.Models;
using WymianaKsiazek.Models.MapperModels;

namespace WymianaKsiazek.Queries.UserQueries
{
    public interface IUserQueries
    {
        Task<UserEntity> GetUserEntity(string userid);
        Task<UserMP> GetUserById(string id); 
        Task<int> CreateNewUser(CreateUserMP model);
        (string, DateTime) CreateAccessToken(UserEntity user);
        Task<string> CreateRefreshToken(string userId);
        Task<TokenOutput> CreateToken(string email, string password);
        Task<int> RemoveRefreshToken(string token);
        Task<MyProfileMP> MyProfile(string userid);
        Task SendEmailResetPassword(UserEntity user);
        Task ChangePassword(UserEntity user, string password);
        Task<int> ChangeCurrentPassword(string userid, string oldpasswd, string newpasswd);
        Task<UserProfile> GetUserProfile(string id);
        Task<List<OffersListMP>> GetUsersLikedOffers(string userid);
    }
}
