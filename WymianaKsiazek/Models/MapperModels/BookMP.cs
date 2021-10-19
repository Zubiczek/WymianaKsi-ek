using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WymianaKsiazek.Database.Entities;

namespace WymianaKsiazek.Models.MapperModels
{
    public class BookMP
    {
        public long Book_Id { get; set; }
        public string Title { get; set; }
        public string Author { get; set; }
        public virtual CategoryMP Category { get; set; }
    }
}
