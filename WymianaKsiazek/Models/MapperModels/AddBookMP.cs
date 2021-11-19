using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WymianaKsiazek.Models.MapperModels
{
    public class AddBookMP
    {
        public long Book_Id { get; set; }
        public string Title { get; set; }
        public string Author { get; set; }
        public long Id { get; set; }
        public string Name { get; set; }
        public AddBookMP(long bookid, string title, string author, long categoryid, string categoryname)
        {
            this.Book_Id = bookid;
            this.Title = title;
            this.Author = author;
            this.Id = categoryid;
            this.Name = categoryname;
        }
    }
}
