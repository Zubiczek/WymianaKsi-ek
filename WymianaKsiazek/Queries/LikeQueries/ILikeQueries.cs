using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WymianaKsiazek.Queries.LikeQueries
{
    public interface ILikeQueries
    {
        public Task<bool> IsOfferFollowedByUser(long offerid, string userid);
        public Task LikeOffer(string userid, long offerid);
        public Task UnLikeOffer(string userid, long offerid);
    }
}
