using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WymianaKsiazek.Database.Entities
{
    public class OfferCommentsEntity
    {
        public long Comment_Id { get; set; }
        [Required(ErrorMessage = "Komentarz nie może być pusty!")]
        [MaxLength(100, ErrorMessage = "Komentarz może mieć maksymalnie 100 znaków!")]
        public string Content { get; set; }
        public string User_Id { get; set; }
        public long Offer_Id { get; set; }
        public virtual UserEntity User { get; set; }
        public virtual OfferEntity Offer { get; set; }
    }
}
