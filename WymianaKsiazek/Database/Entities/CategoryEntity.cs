using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WymianaKsiazek.Database.Entities
{
    public class CategoryEntity
    {
        public long Category_Id { get; set; }
        public string Category_Name { get; set; }
        public virtual ICollection<BookEntity> Books { get; set; }
        public CategoryEntity()
        {
            Books = new List<BookEntity>();
        }
    }
}
