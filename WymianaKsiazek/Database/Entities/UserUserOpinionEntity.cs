using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WymianaKsiazek.Database.Entities
{
    public class UserUserOpinionEntity
    {
        public string User_Id { get; set; }
        public virtual UserEntity User { get; set; }
        public long Opinion_Id { get; set; }
        public virtual UserOpinionsEntity UserOpinion { get; set; }
        public int Value { get; set; }
    }
}
