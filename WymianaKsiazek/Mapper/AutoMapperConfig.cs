using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using WymianaKsiazek.Database.Entities;
using WymianaKsiazek.Models.MapperModels;

namespace WymianaKsiazek.Mapper
{
    public static class AutoMapperConfig
    {
        public static IMapper Initialize()
            => new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<OfferEntity, OffersListMP>();
                cfg.CreateMap<OfferEntity, OfferMP>();
                cfg.CreateMap<AddressEntity, AddressMP>();
                cfg.CreateMap<BookEntity, BookMP>();
                cfg.CreateMap<BookEntity, BookOpinionMP>();
                cfg.CreateMap<BookOpinionsEntity, BookOpinionsMP>();
                cfg.CreateMap<CategoryEntity, CategoryMP>();
                cfg.CreateMap<OfferCommentsEntity, OfferCommentsMP>();
                cfg.CreateMap<BookCommentsEntity, BookCommentsMP>();
                cfg.CreateMap<UserEntity, UserMP>();
                cfg.CreateMap<UserEntity, MyProfileMP>();
                cfg.CreateMap<UserEntity, UserProfile>();
                cfg.CreateMap<BookEntity, BookReviewsList>();
                cfg.CreateMap<UserEntity, UserWithOpinionsMP>();
                cfg.CreateMap<UserOpinionsEntity, UserOpinionsMP>();
                cfg.CreateMap<ConversationEntity, ConversationMP>();
                cfg.CreateMap<MessageEntity, MessageMP>();
                cfg.CreateMap<MessageEntity, LastMessageMP>();
                cfg.CreateMap<ConversationEntity, UserConversationsMP>();
            }).CreateMapper();
    }
}
