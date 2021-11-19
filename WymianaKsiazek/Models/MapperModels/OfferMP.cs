using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WymianaKsiazek.Database.Entities;

namespace WymianaKsiazek.Models.MapperModels
{
    public class OfferMP
    {
        public long Offer_Id { get; set; }
        public string Content { get; set; }
        public bool ForSale { get; set; }
        public decimal Price { get; set; }
        public DateTime CreatedOn { get; set; }
        public string Image { get; set; }
        public virtual UserWithOpinionsMP User { get; set; }
        public virtual BookMP Book { get; set; }
        public virtual AddressMP Address { get; set; }
        public virtual ICollection<OfferCommentsMP> Comments { get; set; }
    }
}
