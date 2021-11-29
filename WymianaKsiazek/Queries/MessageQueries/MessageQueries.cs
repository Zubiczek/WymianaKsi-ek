using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WymianaKsiazek.Database;
using WymianaKsiazek.Database.Entities;
using WymianaKsiazek.Models.MapperModels;

namespace WymianaKsiazek.Queries.MessageQueries
{
    public class MessageQueries : IMessageQueries
    {
        private readonly IMapper _mapper;
        private readonly Context _context;
        public MessageQueries(IMapper mapper, Context context)
        {
            _mapper = mapper;
            _context = context;
        }
        public async Task<ConversationMP> CheckIfConversationExist(string userid1, string userid2)
        {
            var conversation = await _context.Conversations.Where(x => ( x.User1_Id == userid1 && x.User2_Id == userid2)
            || (x.User1_Id == userid2 && x.User2_Id == userid1)).FirstOrDefaultAsync();
            return _mapper.Map<ConversationMP>(conversation);
        }
        public async Task<ConversationMP> GetConversationById(long id, string userid)
        {
            var conversation = _context.Conversations.Include(x => x.User1).Include(x => x.User2).Include(x => x.Messages)
                .ThenInclude(x => x.User).Where(x => x.Id == id).FirstOrDefault();
            if (conversation != null)
            {
                if (userid != conversation.LastMessageFrom_UserId)
                {
                    conversation.IsAllMessagesRead = true;
                    await _context.SaveChangesAsync();
                }
            }
            return _mapper.Map<ConversationMP>(conversation);
        }
        public async Task CreateMessage(long convid, string text, string userid, string otheruserid)
        {
            var message = new MessageEntity();
            using (var transaction = _context.Database.BeginTransaction())
            {
                var conv = _context.Conversations.Where(x => x.Id == convid).FirstOrDefault();
                if(conv != null)
                {
                    conv.LastMessageFrom_UserId = userid;
                }
                var status = _context.RefreshTokens.Where(x => x.UserId == otheruserid).Select(x => x.IsUserActiveInChat).FirstOrDefault();
                if(status == true) conv.IsAllMessagesRead = true;
                else conv.IsAllMessagesRead = false;
                message.Text = text;
                message.Conv_Id = convid;
                message.User_Id = userid;
                _context.Add(message);
                await _context.SaveChangesAsync();
                transaction.Commit();
            }
        }
        public async Task CreateConversation(string userid1, string userid2)
        {
            var conversation = new ConversationEntity();
            using (var transaction = _context.Database.BeginTransaction())
            {
                conversation.User1_Id = userid1;
                conversation.User2_Id = userid2;
                conversation.IsAllMessagesRead = false;
                conversation.LastMessageFrom_UserId = userid1;
                _context.Add(conversation);
                await _context.SaveChangesAsync();
                transaction.Commit();
            }
        }
        public async Task CreateFirstMessage(long convid, string userid, string text)
        {
            var message = new MessageEntity();
            using (var transaction = _context.Database.BeginTransaction())
            {
                message.Text = text;
                message.Conv_Id = convid;
                message.User_Id = userid;
                _context.Add(message);
                await _context.SaveChangesAsync();
                transaction.Commit();
            }
        }
        public async Task<string> GetUserConnectionId(string userid)
        {
            var connectionid = await _context.RefreshTokens.Where(x => x.UserId == userid)
                .Select(x => x.ChatConnectionId).FirstOrDefaultAsync();
            return connectionid;
        }
        public async Task AddUserConnectionId(string userid, string id)
        {
            var tokenentity = await _context.RefreshTokens.Where(x => x.UserId == userid).FirstOrDefaultAsync();
            if (tokenentity != null)
            {
                using (var transaction = _context.Database.BeginTransaction())
                {
                    tokenentity.ChatConnectionId = id;
                    await _context.SaveChangesAsync();
                    transaction.Commit();
                }
            }
        }
        public async Task<List<UserConversationsMP>> GetUserConversations(string userid)
        {
            List<UserConversationsMP> userconversations = new List<UserConversationsMP>();
            var myconversations = await _context.Conversations.Include(x => x.User1).Include(x => x.User2).Include(x => x.Messages)
                .Where(x => x.User1_Id == userid || x.User2_Id == userid).ToListAsync();
            foreach(var i in myconversations)
            {
                var conversation = _mapper.Map<UserConversationsMP>(i);
                var lastmessage = (from m in i.Messages orderby m.SendDate descending select m).FirstOrDefault();
                var message = _mapper.Map<LastMessageMP>(lastmessage);
                conversation.LastMessage = message;
                userconversations.Add(conversation);
            }
            return userconversations;
        }
        public async Task ChangeUserStatus(string userid, bool p)
        {
            using(var transaction = _context.Database.BeginTransaction())
            {
                var tokenentity = _context.RefreshTokens.Where(x => x.UserId == userid).FirstOrDefault();
                if(tokenentity != null)
                {
                    tokenentity.IsUserActiveInChat = p;
                    await _context.SaveChangesAsync();
                    transaction.Commit();
                }
            }
        }
    }
}
