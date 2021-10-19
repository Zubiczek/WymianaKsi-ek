using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WymianaKsiazek.Database.Entities
{
    public class AddressEntity
    {
        public long Address_Id { get; set; }
        public string Name { get; set; }
        public string Province { get; set; }
        public string District { get; set; }
        public string Community { get; set; }
        public virtual ICollection<OfferEntity> Offers { get; set; }
        public AddressEntity()
        {
            Offers = new List<OfferEntity>();
        }
    }
}
