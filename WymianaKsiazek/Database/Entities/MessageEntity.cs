using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WymianaKsiazek.Database.Entities
{
    public class MessageEntity
    {
        public long Id { get; set; }
        [Required]
        [MaxLength(150)]
        public string Text { get; set; }
        public DateTime SendDate { get; set; }
        public string User_Id { get; set; }
        public virtual UserEntity User { get; set; }
        public long Conv_Id { get; set; }
        public virtual ConversationEntity Conversation { get; set; }
        public MessageEntity()
        {
            SendDate = DateTime.UtcNow;
        }
    }
}
