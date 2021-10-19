using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WymianaKsiazek.Models.MapperModels;

namespace WymianaKsiazek.Queries.OfferQueries
{
    public interface IOfferQueries
    {
        public List<OffersListMP> GetAllOffers();
        public List<OffersListMP> GetCityOffers(long addressid);
        public List<OffersListMP> GetUserOffers(string userid);
        public List<OffersListMP> GetSearchedOffers(string bookname);
        public OfferMP GetOfferById(long offerid);
        public void AddOffer(CreateOfferMP offermodel, string userid);
        public void AddCommentToOffer(string comment, string userid, long offerid);
    }
}
