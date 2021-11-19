using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WymianaKsiazek.Database.Entities
{
    public class BookEntity
    {
        public long Book_Id { get; set; }
        [Required(ErrorMessage = "Tytuł jest wymagany!")]
        [MaxLength(40, ErrorMessage = "Tytuł może mieć maksymalnie 40 znaków!")]
        public string Title { get; set; }
        [Required(ErrorMessage ="Autor jest wymagany!")]
        [MaxLength(60, ErrorMessage = "Autor może mieć maksymalnie 60 znaków!")]
        public string Author { get; set; }
        [StringLength(13, ErrorMessage = "Kod ISBN musi zawierać 13 znaków!")]
        public string ISBN { get; set; }
        public string Img { get; set; }
        public long Category_Id { get; set; }
        public virtual CategoryEntity Category { get; set; }
        public virtual BookOpinionsEntity Opinion { get; set; }
        public virtual ICollection<OfferEntity> Offers { get; set; }
        public virtual ICollection<BookCommentsEntity> Comments { get; set; }
        public BookEntity()
        {
            Offers = new List<OfferEntity>();
            Comments = new List<BookCommentsEntity>();
        }
    }
}
