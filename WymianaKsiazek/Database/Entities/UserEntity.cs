using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WymianaKsiazek.Database.Entities
{
    public class UserEntity : IdentityUser
    {
        public long Address_Id { get; set; }
        public virtual AddressEntity Address { get; set; }
        public virtual ICollection<OfferEntity> Offers { get; set; }
        public virtual ICollection<OfferCommentsEntity> OfferComments { get; set; }
        public virtual ICollection<BookCommentsEntity> BookComments { get; set; }
        public virtual ICollection<UserBookOpinion> UserOpinions { get; set; }
        public virtual ICollection<UserUserOpinionEntity> UserUserOpinion { get; set; }
        public virtual UserOpinionsEntity UserOpinion { get; set; }
        public virtual ICollection<UserLikedOffersEntity> LikedOffers { get; set; }
        public virtual ICollection<ConversationEntity> MyConversations { get; set; }
        public virtual ICollection<ConversationEntity> ConversationsWith { get; set; }
        public virtual ICollection<MessageEntity> Messages { get; set; }
        public UserEntity()
        {
            Offers = new List<OfferEntity>();
            OfferComments = new List<OfferCommentsEntity>();
            BookComments = new List<BookCommentsEntity>();
            UserOpinions = new List<UserBookOpinion>();
            UserUserOpinion = new List<UserUserOpinionEntity>();
            LikedOffers = new List<UserLikedOffersEntity>();
            MyConversations = new List<ConversationEntity>();
            ConversationsWith = new List<ConversationEntity>();
            Messages = new HashSet<MessageEntity>();
        }
    }
}
