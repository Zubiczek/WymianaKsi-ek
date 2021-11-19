using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WymianaKsiazek.Models.MapperModels
{
    public class BookReviewsList
    {
        public long Book_Id { get; set; }
        public string Title { get; set; }
        public string Author { get; set; }
        public string Img { get; set; }
        public long Category_Id { get; set; }
        public virtual CategoryMP Category { get; set; }
        public virtual BookOpinionsMP Opinion { get; set; }
    }
}
