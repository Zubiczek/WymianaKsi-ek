using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WymianaKsiazek.Database.Entities;

namespace WymianaKsiazek.Models.MapperModels
{
    public class OffersListMP
    {
        public long Offer_Id { get; set; }
        public bool ForSale { get; set; }
        public decimal Price { get; set; }
        public string Image { get; set; }
        public virtual UserMP User { get; set; }
        public virtual BookMP Book { get; set; }
        public virtual AddressMP Address { get; set; }
    }
}
