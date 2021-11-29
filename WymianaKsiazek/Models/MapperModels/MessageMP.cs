using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WymianaKsiazek.Models.MapperModels
{
    public class MessageMP
    {
        public long Id { get; set; }
        public string Text { get; set; }
        public DateTime SendDate { get; set; }
        public virtual UserMP User { get; set; }
    }
}
