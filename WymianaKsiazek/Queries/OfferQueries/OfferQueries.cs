using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Security.Claims;
using WymianaKsiazek.Database;
using WymianaKsiazek.Database.Entities;
using WymianaKsiazek.Models.MapperModels;

namespace WymianaKsiazek.Queries.OfferQueries
{
    public class OfferQueries : IOfferQueries
    {
        private readonly Context _context;
        private readonly IMapper _mapper;
        private readonly UserManager<UserEntity> _userManager;
        public OfferQueries(Context context, IMapper mapper, UserManager<UserEntity> userManager)
        {
            _context = context;
            _mapper = mapper;
            _userManager = userManager;
        }
        public List<OffersListMP> GetAllOffers()
        {
            var offerentites = _context.Offer.Include(x => x.User).Include(x => x.Book).Include(x => x.Address).ToList();
            return _mapper.Map<List<OffersListMP>>(offerentites);
        }
        public List<OffersListMP> GetCityOffers(long addressid)
        {
            var offers = _context.Offer.Include(x => x.User).Include(x => x.Book).Include(x => x.Address).Where(x => x.Address_Id == addressid).ToList();
            return _mapper.Map<List<OffersListMP>>(offers);
        }
        public List<OffersListMP> GetUserOffers(string userid)
        {
            var offers = _context.Offer.Include(x => x.User).Include(x => x.Book).Include(x => x.Address).Where(x => x.User_Id == userid).ToList();
            return _mapper.Map<List<OffersListMP>>(offers);
        }
        public List<OffersListMP> GetSearchedOffers(string bookname)
        {
            var bookid = _context.Book.Where(x => EF.Functions.Like(x.Title, "%"+bookname+"%")).Select(x => x.Book_Id).FirstOrDefault();
            var offers = _context.Offer.Include(x => x.User).Include(x => x.Book).Include(x => x.Address).Where(x => x.Book_Id == bookid).ToList();
            return _mapper.Map<List<OffersListMP>>(offers);
        }
        public OfferMP GetOfferById(long offerid)
        {
            var offer = _context.Offer.Include(x => x.User).Include(x => x.Book).Include(x => x.Address).Include(x => x.Comments).Where(x => x.Offer_Id == offerid).FirstOrDefault();
            return _mapper.Map<OfferMP>(offer);
        }
        public void AddOffer(CreateOfferMP offermodel, string userid)
        {
            var checkbook = _context.Book.Where(x => x.Author == offermodel.Book.Author && x.Title == offermodel.Book.Title).FirstOrDefault();
            long id;
            if (checkbook == null)
            {
                var book = new BookEntity { Title = offermodel.Book.Title, Author = offermodel.Book.Author, Category_Id = offermodel.Book.Category_Id, ISBN = offermodel.Book.ISBN };
                _context.Book.Add(book);
                _context.SaveChangesAsync();
                var book2 = _context.Book.Where(x => x.Author == offermodel.Book.Author && x.Title == offermodel.Book.Title).FirstOrDefault();
                id = book2.Book_Id; ;
            }
            else
            {
                id = checkbook.Book_Id;
            }
            long addressid = _context.User.Where(x => x.Id == userid).Select(x => x.Address_Id).FirstOrDefault();
            var offer = new OfferEntity { Content = offermodel.Content, Price = offermodel.Price, Book_Id = id, ForSale = offermodel.ForSale, Address_Id = addressid, User_Id = userid, Image = offermodel.Image};
            _context.Offer.Add(offer);
            _context.SaveChangesAsync();
        }
        public void AddCommentToOffer(string comment, string userid, long offerid)
        {
            OfferCommentsEntity commententity = new OfferCommentsEntity();
            commententity.Content = comment;
            commententity.User_Id = userid;
            commententity.Offer_Id = offerid;
            _context.OfferComments.Add(commententity);
            _context.SaveChanges();
        }
    }
}
