using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace WymianaKsiazek.Database.Entities
{
    public class BookOpinionsEntity
    {
        public long Opinion_Id { get; set; }
        [DefaultValue(0)]
        public uint Opinion5 { get; set; }
        [DefaultValue(0)]
        public uint Opinion4 { get; set; }
        [DefaultValue(0)]
        public uint Opinion3 { get; set; }
        [DefaultValue(0)]
        public uint Opinion2 { get; set; }
        [DefaultValue(0)]
        public uint Opinion1 { get; set; }
        public long Book_Id { get; set; }
        public virtual BookEntity Book { get; set; }
        public ICollection<UserBookOpinion> UserOpinions { get; set; }
        public BookOpinionsEntity()
        {
            UserOpinions = new List<UserBookOpinion>();
        }
    }
}
