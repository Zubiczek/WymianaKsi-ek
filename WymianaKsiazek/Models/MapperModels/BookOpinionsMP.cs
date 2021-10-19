using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WymianaKsiazek.Models.MapperModels
{
    public class BookOpinionsMP
    {
        public long Opinion_Id { get; set; }
        public uint Opinion5 { get; set; }
        public uint Opinion4 { get; set; }
        public uint Opinion3 { get; set; }
        public uint Opinion2 { get; set; }
        public uint Opinion1 { get; set; }
    }
}
