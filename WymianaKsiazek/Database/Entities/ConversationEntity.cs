using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WymianaKsiazek.Database.Entities
{
    public class ConversationEntity
    {
        public long Id { get; set; }
        public string User1_Id { get; set; }
        public virtual UserEntity User1 { get; set; }
        public string User2_Id { get; set; }    
        public virtual UserEntity User2 { get; set; }
        public bool IsAllMessagesRead { get; set; }
        public string LastMessageFrom_UserId { get; set; }
        public virtual ICollection<MessageEntity> Messages { get; set; }
    }
}
