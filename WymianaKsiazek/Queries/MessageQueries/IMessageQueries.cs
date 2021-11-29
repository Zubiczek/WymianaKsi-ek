using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WymianaKsiazek.Models.MapperModels;

namespace WymianaKsiazek.Queries.MessageQueries
{
    public interface IMessageQueries
    {
        public Task<ConversationMP> CheckIfConversationExist(string userid1, string userid2);
        public Task<ConversationMP> GetConversationById(long id, string userid);
        public Task CreateMessage(long convid, string text, string userid, string otheruserid);
        public Task CreateConversation(string userid1, string userid2);
        public Task CreateFirstMessage(long convid, string userid, string text);
        public Task<string> GetUserConnectionId(string userid);
        public Task AddUserConnectionId(string userid, string id);
        public Task<List<UserConversationsMP>> GetUserConversations(string userid);
        public Task ChangeUserStatus(string userid, bool p);
    }
}
