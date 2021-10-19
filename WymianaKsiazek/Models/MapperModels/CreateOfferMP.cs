using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WymianaKsiazek.Models.MapperModels
{
    public class CreateOfferMP
    {
        public string Content { get; set; }
        public bool ForSale { get; set; }
        public decimal Price { get; set; }
        public string Image { get; set; }
        public CreateBookMP Book { get; set; }
    }
}
