using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WymianaKsiazek.Database.Entities;

namespace WymianaKsiazek.Models.MapperModels
{
    public class OfferCommentsMP
    {
        public long Comment_Id { get; set; }
        public string Content { get; set; }
        public virtual UserMP User { get; set; }
    }
}
