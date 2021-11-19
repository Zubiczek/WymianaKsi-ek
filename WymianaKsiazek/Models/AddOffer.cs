using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WymianaKsiazek.Models
{
    public class AddOffer
    {
        public string titleinput { get; set; }
        public string authorinput { get; set; }
        public long inputcategoryid { get; set; }
        public string addoffercontent {get; set;}
        public int forsale { get; set; }
        public string price { get; set; }
        public IFormFile addofferimg { get; set; }
    }
}
