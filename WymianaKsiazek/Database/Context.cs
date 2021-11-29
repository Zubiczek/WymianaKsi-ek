using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using WymianaKsiazek.Database.Entities;

namespace WymianaKsiazek.Database
{
    public class Context : IdentityDbContext
    {
        public Context(DbContextOptions<Context> options) :base(options)
        {

        }
        public DbSet<AddressEntity> Address { get; set; }
        public DbSet<CategoryEntity> Category { get; set; }
        public DbSet<UserEntity> User { get; set; }
        public DbSet<BookEntity> Book { get; set; }
        public DbSet<OfferEntity> Offer { get; set; }
        public DbSet<BookOpinionsEntity> BookOpinions { get; set; }
        public DbSet<BookCommentsEntity> BookComments { get; set; }
        public DbSet<OfferCommentsEntity> OfferComments { get; set; }
        public DbSet<UserBookOpinion> UserBookOpinions { get; set; }
        public DbSet<RefreshTokenEntity> RefreshTokens { get; set; }
        public DbSet<UserOpinionsEntity> UserOpinions { get; set; }
        public DbSet<UserUserOpinionEntity> UserOpinionInfo { get; set; }
        public DbSet<UserLikedOffersEntity> UserLikedOffers { get; set; }
        public DbSet<ConversationEntity> Conversations { get; set; }
        public DbSet<MessageEntity> Messages { get; set; }
        public DbSet<ReportReasonsEntity> ReportReasons { get; set; }
        public DbSet<UserReportsEntity> UserReports { get; set; }
        public DbSet<OfferReportsEntity> OfferReports { get; set; }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            //OfferEntity
            builder.Entity<OfferEntity>().HasKey(x => x.Offer_Id);
            builder.Entity<OfferEntity>().HasOne(x => x.User).WithMany(x => x.Offers).HasForeignKey(x => x.User_Id);
            builder.Entity<OfferEntity>().HasOne(x => x.Book).WithMany(x => x.Offers).HasForeignKey(x => x.Book_Id);
            builder.Entity<OfferEntity>().HasOne(x => x.Address).WithMany(x => x.Offers).HasForeignKey(x => x.Address_Id);
            //BookEntity
            builder.Entity<BookEntity>().HasKey(x => x.Book_Id);
            builder.Entity<BookEntity>().HasOne(x => x.Category).WithMany(x => x.Books).HasForeignKey(x => x.Category_Id);
            //AddressEntity
            builder.Entity<AddressEntity>().HasKey(x => x.Address_Id);
            //CategoryEntity
            builder.Entity<CategoryEntity>().HasKey(x => x.Category_Id);
            //UserEntity
            builder.Entity<UserEntity>().HasOne(x => x.Address);
            //BookOpinionsEntity
            builder.Entity<BookOpinionsEntity>().HasKey(x => x.Opinion_Id);
            builder.Entity<BookOpinionsEntity>().HasOne(x => x.Book).WithOne(x => x.Opinion).HasForeignKey<BookOpinionsEntity>(x => x.Book_Id);
            //BookCommentsEntity
            builder.Entity<BookCommentsEntity>().HasKey(x => x.Comment_Id);
            builder.Entity<BookCommentsEntity>().HasOne(x => x.User).WithMany(x => x.BookComments).HasForeignKey(x => x.User_Id);
            builder.Entity<BookCommentsEntity>().HasOne(x => x.Book).WithMany(x => x.Comments).HasForeignKey(x => x.Book_Id);
            //OfferCommentsEntity
            builder.Entity<OfferCommentsEntity>().HasKey(x => x.Comment_Id);
            builder.Entity<OfferCommentsEntity>().HasOne(x => x.User).WithMany(x => x.OfferComments).HasForeignKey(x => x.User_Id);
            builder.Entity<OfferCommentsEntity>().HasOne(x => x.Offer).WithMany(x => x.Comments).HasForeignKey(x => x.Offer_Id);
            //UserBookOpinion
            builder.Entity<UserBookOpinion>().HasKey(x => new { x.User_Id, x.Opinion_Id });
            builder.Entity<UserBookOpinion>().HasOne(x => x.User).WithMany(x => x.UserOpinions).HasForeignKey(x => x.User_Id);
            builder.Entity<UserBookOpinion>().HasOne(x => x.Opinion).WithMany(x => x.UserOpinions).HasForeignKey(x => x.Opinion_Id);
            //RefreshToken
            builder.Entity<RefreshTokenEntity>().HasKey(x => x.Id);
            builder.Entity<RefreshTokenEntity>().HasOne(x => x.User).WithMany().OnDelete(DeleteBehavior.Cascade);
            //UserOpinions
            builder.Entity<UserOpinionsEntity>().HasKey(x => x.Opinion_Id);
            builder.Entity<UserOpinionsEntity>().HasOne(x => x.User).WithOne(x => x.UserOpinion).HasForeignKey<UserOpinionsEntity>(x => x.User_Id);
            //UserUserOpinion
            builder.Entity<UserUserOpinionEntity>().HasKey(x => new { x.User_Id, x.Opinion_Id});
            builder.Entity<UserUserOpinionEntity>().HasOne(x => x.User).WithMany(x => x.UserUserOpinion).HasForeignKey(x => x.User_Id);
            builder.Entity<UserUserOpinionEntity>().HasOne(x => x.UserOpinion).WithMany(x => x.UserUserOpinions).HasForeignKey(x => x.Opinion_Id);
            //UserLikedOffers
            builder.Entity<UserLikedOffersEntity>().HasKey(x => new { x.User_Id, x.Offer_Id });
            builder.Entity<UserLikedOffersEntity>().HasOne(x => x.User).WithMany(x => x.LikedOffers).HasForeignKey(x => x.User_Id);
            builder.Entity<UserLikedOffersEntity>().HasOne(x => x.Offer).WithMany(x => x.UserLiked).HasForeignKey(x => x.Offer_Id);
            //ConversationEntity
            builder.Entity<ConversationEntity>().HasKey(x => x.Id);
            builder.Entity<ConversationEntity>().HasOne(x => x.User1).WithMany(x => x.MyConversations).HasForeignKey(x => x.User1_Id);
            builder.Entity<ConversationEntity>().HasOne(x => x.User2).WithMany(x => x.ConversationsWith).HasForeignKey(x => x.User2_Id);
            //MessageEntity
            builder.Entity<MessageEntity>().HasKey(x => x.Id);
            builder.Entity<MessageEntity>().HasOne(x => x.User).WithMany(x => x.Messages).HasForeignKey(x => x.User_Id);
            builder.Entity<MessageEntity>().HasOne(x => x.Conversation).WithMany(x => x.Messages).HasForeignKey(x => x.Conv_Id);
            //ReportReasonsEntity
            builder.Entity<ReportReasonsEntity>().HasKey(x => x.Reason_Id);
            //UserReportsEntity
            builder.Entity<UserReportsEntity>().HasKey(x => x.Id);
            builder.Entity<UserReportsEntity>().HasOne(x => x.Reason).WithMany(x => x.UserReports).HasForeignKey(x => x.Reason_Id);
            builder.Entity<UserReportsEntity>().HasOne(x => x.ReportedUser).WithMany(x => x.BeenReported).HasForeignKey(x => x.ReportedUser_Id);
            builder.Entity<UserReportsEntity>().HasOne(x => x.UserWhoReported).WithMany(x => x.Reported).HasForeignKey(x => x.UserWhoReported_Id);
            //OfferReportsEntity
            builder.Entity<OfferReportsEntity>().HasKey(x => x.Id);
            builder.Entity<OfferReportsEntity>().HasOne(x => x.Reason).WithMany(x => x.OfferReports).HasForeignKey(x => x.Reason_Id);
            builder.Entity<OfferReportsEntity>().HasOne(x => x.Offer).WithMany(x => x.Reports).HasForeignKey(x => x.Offer_Id);
            builder.Entity<OfferReportsEntity>().HasOne(x => x.User).WithMany(x => x.ReportedOffers).HasForeignKey(x => x.User_Id);
        }
    }
}
