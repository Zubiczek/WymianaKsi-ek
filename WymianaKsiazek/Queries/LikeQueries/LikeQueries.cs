using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WymianaKsiazek.Database;
using Microsoft.EntityFrameworkCore;
using WymianaKsiazek.Database.Entities;

namespace WymianaKsiazek.Queries.LikeQueries
{
    public class LikeQueries : ILikeQueries
    {
        private readonly Context _context;
        public LikeQueries(Context context)
        {
            _context = context;
        }
        public async Task<bool> IsOfferFollowedByUser(long offerid, string userid)
        {
            var likedoffer = await _context.UserLikedOffers.Where(x => x.Offer_Id == offerid && x.User_Id == userid).FirstOrDefaultAsync();
            if (likedoffer == null)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
        public async Task LikeOffer(string userid, long offerid)
        {
            UserLikedOffersEntity like = new UserLikedOffersEntity();
            like.User_Id = userid;
            like.Offer_Id = offerid;
            using (var transaction = _context.Database.BeginTransaction())
            {
                _context.Add(like);
                await _context.SaveChangesAsync();
                transaction.Commit();
            }
        }
        public async Task UnLikeOffer(string userid, long offerid)
        {
            var like = _context.UserLikedOffers.Where(x => x.User_Id == userid && x.Offer_Id == offerid).FirstOrDefault();
            if (like != null)
            {
                _context.UserLikedOffers.Remove(like);
                await _context.SaveChangesAsync();
            }
        }
    }
}
