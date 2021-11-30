using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WymianaKsiazek.Models.MapperModels
{
    public class UserProfile
    {
        public string Id { get; set; }
        public string UserName { get; set; }
        public string Img { get; set; }
        public virtual UserOpinionsMP UserOpinion { get; set; }
        public virtual ICollection<OffersListMP> Offers { get; set; }
    }
}
