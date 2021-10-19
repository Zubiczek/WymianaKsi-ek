using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WymianaKsiazek.Database.Entities
{
    public class UserBookOpinion
    {
        public string User_Id { get; set; }
        public virtual UserEntity User { get; set; }
        public long Opinion_Id { get; set; }
        public virtual BookOpinionsEntity Opinion { get; set; }
        public int Value { get; set; }
    }
}
