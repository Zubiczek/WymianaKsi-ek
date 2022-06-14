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
using WymianaKsiazek.Models;
using System.IO;
using System.Globalization;

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

        public async Task<List<OffersListMP>> GetOffers()
        {
            var offerentites = await _context.Offer.Include(x => x.User).Include(x => x.Book).ThenInclude(x => x.Category).Include(x => x.Address).ToListAsync();
            return _mapper.Map<List<OffersListMP>>(offerentites);
        }
        public async Task<List<OffersListMP>> GetAllOffers()
        {
            var offerentites = await _context.Offer.Include(x => x.User).
                Include(x => x.Book).Include(x => x.Address).OrderByDescending(x => x.CreatedOn).Take(12).ToListAsync();
            return _mapper.Map<List<OffersListMP>>(offerentites);
        }
        public async Task<List<OffersListMP>> GetOffersWithBook(string title, string author)
        {
            var offers = await _context.Offer.Include(x => x.User).Include(x => x.Book).Include(x => x.Address)
                .Where(x => x.Book.Title == title && x.Book.Author == author).ToListAsync();
            return _mapper.Map<List<OffersListMP>>(offers);
        }
        public async Task<List<OffersListMP>> GetUserOffers(string userid)
        {
            var offers = await _context.Offer.Include(x => x.User).Include(x => x.Book).
                Include(x => x.Address).Where(x => x.User_Id == userid).ToListAsync();
            return _mapper.Map<List<OffersListMP>>(offers);
        }
        public async Task<List<OffersListMP>> GetSearchedOffers(string bookname)
        {
            var bookid = await _context.Book.Where(x => EF.Functions.Like(x.Title, "%"+bookname+"%")).Select(x => x.Book_Id).FirstOrDefaultAsync();
            var offers = await _context.Offer.Include(x => x.User).Include(x => x.Book).Include(x => x.Address).Where(x => x.Book_Id == bookid).ToListAsync();
            return _mapper.Map<List<OffersListMP>>(offers);
        }
        public async Task<List<OffersListMP>> GetOffersByCategory(long id)
        {
            var offers = await _context.Offer.Include(x => x.User).Include(x => x.Book).
                Include(x => x.Address).Where(x => x.Book.Category_Id == id).ToListAsync();
            return _mapper.Map<List<OffersListMP>>(offers);
        }
        public async Task<OfferMP> GetOfferById(long offerid)
        {
            var offer = await _context.Offer.Include(x => x.User).ThenInclude(x => x.UserOpinion).
                Include(x => x.Book).Include(x => x.Address).Include(x => x.Comments).
                ThenInclude(x => x.User).Where(x => x.Offer_Id == offerid).FirstOrDefaultAsync();
            return _mapper.Map<OfferMP>(offer);
        }
        public async Task<List<OffersListMP>> GetSearchesOffers(string titleauthor, string city)
        {
            var alloffers = await GetOffers();
            List<OffersListMP> offers = null;
            if((titleauthor == null || titleauthor == "") && (city == null || city == ""))
            {
                return alloffers;
            }
            else if(titleauthor == null || titleauthor == "")
            {
                offers = (from i in alloffers where i.Address.Name == city select i).ToList();
            }
            else
            {
                titleauthor = titleauthor.ToUpper();
                var strings = titleauthor.Split(' ');
                int index = 1;
                if (city != null && city != "")
                {
                    alloffers = (from i in alloffers where i.Address.Name == city select i).ToList();
                }
                if (strings.Length > 1)
                {
                    while (index < strings.Length)
                    {
                        string word1 = "";
                        string word2 = "";
                        for (int j = 0; j < index; j++)
                        {
                            word1 += strings[j];
                            if (j < index - 1)
                            {
                                word1 += " ";
                            }
                        }
                        for (int j = index; j < strings.Length; j++)
                        {
                            word2 += strings[j];
                            if (j < strings.Length - 1)
                            {
                                word2 += " ";
                            }
                        }
                        index++;
                        offers = (from i in alloffers where i.Book.Title.ToUpper().Contains(word1) && i.Book.Author.ToUpper().Contains(word2) select i).ToList();
                        if (offers.Count > 0) return offers;
                        offers = (from i in alloffers where i.Book.Title.ToUpper().Contains(word2) && i.Book.Author.ToUpper().Contains(word1) select i).ToList();
                        if (offers.Count > 0) return offers;
                        /*if (city == null || city == "")
                        {
                            offers = (from i in alloffers where i.Book.Title.ToUpper().Contains(word1) && i.Book.Author.ToUpper().Contains(word2) select i).ToList();
                            if (offers.Count > 0)
                            {
                                return offers;
                            }
                            offers = (from i in alloffers where i.Book.Title.ToUpper().Contains(word2) && i.Book.Author.ToUpper().Contains(word1) select i).ToList();
                            if (offers.Count > 0)
                            {
                                return offers;
                            }
                        }
                        else
                        {
                            offers = (from i in alloffers where i.Book.Title.ToUpper().Contains(word1) && i.Book.Author.ToUpper().Contains(word2) && i.Address.Name == city select i).ToList();
                            if (offers.Count > 0)
                            {
                                return offers;
                            }
                            offers = (from i in alloffers where i.Book.Title.ToUpper().Contains(word2) && i.Book.Author.ToUpper().Contains(word1) && i.Address.Name == city select i).ToList();
                            if (offers.Count > 0)
                            {
                                return offers;
                            }
                        }*/
                    }
                }
                offers = (from i in alloffers where i.Book.Title.ToUpper().Contains(titleauthor) select i).ToList();
                if (offers.Count > 0) return offers;
                else
                {
                    offers = (from i in alloffers where i.Book.Author.ToUpper().Contains(titleauthor) select i).ToList();
                    if (offers.Count > 0) return offers;
                }
            }
            return offers;
        }
        public List<OffersListMP> GetSearchedOffersByFilters(List<OffersListMP> offers, long categoryid, uint lowprice, uint upprice, int type)
        {
            if(categoryid > 0)
            {
                offers = (from i in offers where i.Book.Category.Category_Id == categoryid select i).ToList();
            }
            if (type != 2)
            {
                if (type == 1)
                {
                    offers = (from i in offers where i.ForSale == true select i).ToList();
                }
                else if (type == 0)
                {
                    offers = (from i in offers where i.ForSale == false select i).ToList();
                }
            }
            if (lowprice > 0 && type != 0)
            {
                offers = (from i in offers where i.Price >= lowprice select i).ToList();
            }
            if (upprice > 0 && type != 0)
            {
                offers = (from i in offers where i.Price <= upprice select i).ToList();
            }
            return offers;
        }
        public async Task<List<AddBookMP>> GetBooksToAdd(string title)
        {
            if (title == null || title == "")
            {
                return null;
            }
            title = title.ToUpper();
            var books = await _context.Book.Include(x => x.Category).Where(x => x.Title.ToUpper().Contains(title)).ToListAsync();
            List<AddBookMP> list = new List<AddBookMP>();
            foreach(var i in books)
            {
                list.Add(new AddBookMP(i.Book_Id, i.Title, i.Author, i.Category_Id, i.Category.Category_Name));
            }
            return list;
        }
        public bool IsContentValid(string content)
        {
            string contentwithnospaces = "";
            bool p = true;
            for(int i=0; i<content.Length; i++)
            {
                if(content[i] != ' ')
                {
                    contentwithnospaces += content[i];
                }
            }
            if(contentwithnospaces == "")
            {
                return false;
            }
            contentwithnospaces = contentwithnospaces.ToUpper();
            string pathtofile = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/banishedwords.txt");
            using (StreamReader reader = new StreamReader(pathtofile))
            {
                string line;
                while((line = reader.ReadLine()) != null)
                {
                    if(contentwithnospaces.Contains(line))
                    {
                        p = false;
                        break;
                    }
                }
            }
            return p;
        }
        public string SaveImage(IFormFile image)
        {
            if (image != null)
            {
                string imageName = Guid.NewGuid().ToString() + Path.GetExtension(image.FileName);
                string SavePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images", imageName);
                using (var stream = new FileStream(SavePath, FileMode.Create))
                {
                    image.CopyTo(stream);
                }
                return imageName;
            }
            return null;
        }
        public async Task<int> AddOffer(AddOffer offer, string userid)
        {
            var newoffer = new OfferEntity();
            if(!IsContentValid(offer.addoffercontent))
            {
                return 1;
            }
            using(var transaction = _context.Database.BeginTransaction())
            {
                var user = _context.User.Include(x => x.Address).Where(x => x.Id == userid).FirstOrDefault();
                if(user == null) return 2;
                var countusersoffers = _context.Offer.Where(x => x.User_Id == userid).Count();
                if(countusersoffers >= 4) return 3;
                var bookid = _context.Book.Where(x => x.Title == offer.titleinput && x.Author == offer.authorinput).FirstOrDefault();
                if(bookid == null) return 4;
                newoffer.Image = SaveImage(offer.addofferimg);
                newoffer.Book_Id = bookid.Book_Id;
                newoffer.User_Id = userid;
                newoffer.Address_Id = user.Address_Id;
                newoffer.IsitAvailable = true;
                newoffer.Price = Decimal.Parse(offer.price, CultureInfo.InvariantCulture);
                if(offer.forsale == 1)
                {
                    newoffer.ForSale = true;
                }
                else
                {
                    newoffer.ForSale = false;
                }
                newoffer.Content = offer.addoffercontent;
                _context.Add(newoffer);
                await _context.SaveChangesAsync();
                transaction.Commit();
            }
            return 0;
        }
        public async Task<OfferReportMP> GetOfferReport(long id)
        {
            var offer = await _context.Offer.Include(x => x.Book).Include(x => x.User).
                Where(x => x.Offer_Id == id).FirstOrDefaultAsync();
            return _mapper.Map<OfferReportMP>(offer);
        }
    }
}
