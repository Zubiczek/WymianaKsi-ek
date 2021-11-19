using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WymianaKsiazek.Models;
using WymianaKsiazek.Models.MapperModels;

namespace WymianaKsiazek.Queries.OfferQueries
{
    public interface IOfferQueries
    {
        public List<OffersListMP> GetOffers();
        public List<OffersListMP> GetAllOffers();
        public List<OffersListMP> GetOffersWithBook(string title, string author);
        public List<OffersListMP> GetCityOffers(long addressid);
        public List<OffersListMP> GetUserOffers(string userid);
        public List<OffersListMP> GetSearchedOffers(string bookname);
        public List<OffersListMP> GetOffersByCategory(long id);
        public OfferMP GetOfferById(long offerid);
        public void AddCommentToOffer(string comment, string userid, long offerid);
        public List<OffersListMP> GetSearchesOffers(string titleauthor, string city);
        public List<OffersListMP> GetSearchedOffersByFilters(List<OffersListMP> offers, long categoryid, uint lowprice, uint upprice, int type);
        public bool IsOfferFollowedByUser(long offerid, string userid);
        public Task LikeOffer(string userid, long offerid);
        public Task UnLikeOffer(string userid, long offerid);
        public List<AddBookMP> GetBooksToAdd(string title);
        public bool IsContentValid(string content);
        public string SaveImage(IFormFile image);
        public Task<int> AddOffer(AddOffer offer, string userid);
    }
}
