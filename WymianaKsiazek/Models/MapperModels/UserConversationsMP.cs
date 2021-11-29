using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WymianaKsiazek.Models.MapperModels
{
    public class UserConversationsMP
    {
        public long Id { get; set; }
        public virtual UserMP User1 { get; set; }
        public virtual UserMP User2 { get; set; }
        public bool IsAllMessagesRead { get; set; }
        public string LastMessageFrom_UserId { get; set; }
        public LastMessageMP LastMessage { get; set; }
    }
}
