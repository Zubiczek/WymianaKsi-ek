using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WymianaKsiazek.Models.MapperModels
{
    public class OfferReportMP
    {
        public long Offer_Id { get; set; }
        public string Content { get; set; }
        public DateTime CreatedOn { get; set; }
        public string Image { get; set; }
        public virtual BookMP Book { get; set; }
        public virtual UserMP User { get; set; }
    }
}
