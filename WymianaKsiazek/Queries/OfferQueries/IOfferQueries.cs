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
        Task<List<OffersListMP>> GetOffers();
        Task<List<OffersListMP>> GetAllOffers();
        Task<List<OffersListMP>> GetOffersWithBook(string title, string author);
        Task<List<OffersListMP>> GetUserOffers(string userid);
        Task<List<OffersListMP>> GetSearchedOffers(string bookname);
        Task<List<OffersListMP>> GetOffersByCategory(long id);
        Task<OfferMP> GetOfferById(long offerid);
        Task<List<OffersListMP>> GetSearchesOffers(string titleauthor, string city);
        List<OffersListMP> GetSearchedOffersByFilters(List<OffersListMP> offers, long categoryid, uint lowprice, uint upprice, int type);
        Task<List<AddBookMP>> GetBooksToAdd(string title);
        bool IsContentValid(string content);
        string SaveImage(IFormFile image);
        Task<int> AddOffer(AddOffer offer, string userid);
        Task<OfferReportMP> GetOfferReport(long id);
    }
}
