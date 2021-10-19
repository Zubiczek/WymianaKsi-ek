using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WymianaKsiazek.Models.MapperModels
{
    public class BookCommentsMP
    {
        public long Comment_Id { get; set; }
        public string Content { get; set; }
        public virtual UserMP User { get; set; }
    }
}
