using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WymianaKsiazek.Database.Entities;

namespace WymianaKsiazek.Models.MapperModels
{
    public class BookOpinionMP
    {
        public long Book_Id { get; set; }
        public string Title { get; set; }
        public string Author { get; set; }
        public string Img { get; set; }
        public virtual BookOpinionsMP Opinion { get; set; }
        public virtual ICollection<BookCommentsMP> Comments { get; set; }
    }
}
