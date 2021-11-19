using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace WymianaKsiazek.Database.Entities
{
    public class UserOpinionsEntity
    {
        public long Opinion_Id { get; set; }
        [DefaultValue(0)]
        public uint OpinionSumValue { get; set;}
        [DefaultValue(0)]
        public uint TotalOpinions { get; set; }
        public string User_Id { get; set; }
        public virtual UserEntity User { get; set; }
        public ICollection<UserUserOpinionEntity> UserUserOpinions { get; set; }
        public UserOpinionsEntity()
        {
            UserUserOpinions = new List<UserUserOpinionEntity>();
        }
    }
}
