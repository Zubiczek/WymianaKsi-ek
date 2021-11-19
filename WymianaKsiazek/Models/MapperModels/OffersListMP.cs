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
        public DateTime CreatedOn { get; set; }
        public virtual UserMP User { get; set; }
        public virtual BookMP Book { get; set; }
        public virtual AddressMP Address { get; set; }
        public string DateFromat()
        {
            TimeSpan time = this.CreatedOn.Subtract(DateTime.UtcNow);
            int days = time.Days;
            string offerdate = "";
            if (days == 0)
            {
                offerdate = "Dzisiaj " + this.CreatedOn.ToString("HH:mm");
            }
            else if (days == 1)
            {
                offerdate = "Wczoraj " + this.CreatedOn.ToString("HH:mm");
            }
            else if(days >= 365)
            {
                offerdate = this.CreatedOn.ToString("dd.MM.yyyy");
            }
            else
            {
                offerdate = this.CreatedOn.ToString("dddd, dd.MM - HH:mm");
            }
            return offerdate;
        }
        public bool IsLikedByCurrentLoggedInUser(ref List<long> offersids)
        {
            foreach(var i in offersids)
            {
                if(i == this.Offer_Id)
                {
                    return true;
                }
            }
            return false;
        }
    }
}
