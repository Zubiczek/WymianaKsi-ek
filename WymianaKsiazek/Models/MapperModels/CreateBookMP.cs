using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WymianaKsiazek.Models.MapperModels
{
    public class CreateBookMP
    {
        public string Title { get; set; }
        public string Author { get; set; }
        public string ISBN { get; set; }
        public long Category_Id { get; set; }
    }
}
