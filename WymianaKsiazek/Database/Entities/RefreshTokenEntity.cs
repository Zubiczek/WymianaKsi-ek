using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WymianaKsiazek.Database.Entities
{
    public class RefreshTokenEntity
    {
        public long Id { get; set; }

        public DateTime ExpiresOn { get; set; }

        public string Token { get; set; }

        public DateTime CreatedOn { get; set; }

        public DateTime UpdatedOn { get; set; }

        public string UserId { get; set; }
        public virtual UserEntity User { get; set; }
        public bool IsActive(DateTime date)
        {
            bool p = true;
            return p;
        }
    }
}
