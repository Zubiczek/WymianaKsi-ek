using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WymianaKsiazek.Database.Entities
{
    public class OfferEntity
    {
        public long Offer_Id { get; set; }
        [Required(ErrorMessage = "Pole nie może być puste!")]
        [MaxLength(200, ErrorMessage = "Pole może zawierać maksymalnie 200 znaków")]
        public string Content { get; set; }
        public bool ForSale { get; set; }
        [Range(1, 99.99, ErrorMessage = "Cena może być w zakresie od 1 do 99,99zł!")]
        public decimal Price { get; set; }
        [Required(ErrorMessage = "Zdjęcie książki jest wymagane!")]
        public string Image { get; set; }
        [DefaultValue(true)]
        public bool IsitAvailable { get; set; }
        public DateTime CreatedOn { get; set; }
        public long Book_Id { get; set; }
        public string User_Id { get; set; }
        public long Address_Id { get; set; }
        public virtual UserEntity User { get; set; }
        public virtual BookEntity Book { get; set; }
        public virtual AddressEntity Address { get; set; }
        public virtual ICollection<OfferCommentsEntity> Comments { get; set; }
        public OfferEntity()
        {
            CreatedOn = DateTime.UtcNow;
            Comments = new List<OfferCommentsEntity>();
        }
    }
}
