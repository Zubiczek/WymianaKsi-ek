﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WymianaKsiazek.Database.Entities
{
    public class UserLikedOffersEntity
    {
        public string User_Id { get; set; }
        public virtual UserEntity User { get; set; }
        public long Offer_Id { get; set; }
        public virtual OfferEntity Offer { get; set; }
    }
}
