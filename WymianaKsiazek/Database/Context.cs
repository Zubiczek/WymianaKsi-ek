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
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            //OfferEntity
            builder.Entity<OfferEntity>().HasKey(x => x.Offer_Id);
            builder.Entity<OfferEntity>().HasOne(x => x.User).WithMany(x => x.Offers);
            builder.Entity<OfferEntity>().HasOne(x => x.Book).WithMany(x => x.Offers);
            builder.Entity<OfferEntity>().HasOne(x => x.Address).WithMany(x => x.Offers);
            //BookEntity
            builder.Entity<BookEntity>().HasKey(x => x.Book_Id);
            builder.Entity<BookEntity>().HasOne(x => x.Category).WithMany(x => x.Books);
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
            builder.Entity<BookCommentsEntity>().HasOne(x => x.User).WithMany(x => x.BookComments);
            builder.Entity<BookCommentsEntity>().HasOne(x => x.Book).WithMany(x => x.Comments);
            //OfferCommentsEntity
            builder.Entity<OfferCommentsEntity>().HasKey(x => x.Comment_Id);
            builder.Entity<OfferCommentsEntity>().HasOne(x => x.User).WithMany(x => x.OfferComments);
            builder.Entity<OfferCommentsEntity>().HasOne(x => x.Offer).WithMany(x => x.Comments);
            //UserBookOpinion
            builder.Entity<UserBookOpinion>().HasKey(x => new { x.User_Id, x.Opinion_Id });
            builder.Entity<UserBookOpinion>().HasOne(x => x.User).WithMany(x => x.UserOpinions).HasForeignKey(x => x.User_Id);
            builder.Entity<UserBookOpinion>().HasOne(x => x.Opinion).WithMany(x => x.UserOpinions).HasForeignKey(x => x.Opinion_Id);
            //RefreshToken
            builder.Entity<RefreshTokenEntity>().HasKey(x => x.Id);
            builder.Entity<RefreshTokenEntity>().HasOne(x => x.User).WithMany().OnDelete(DeleteBehavior.Cascade);
        }
    }
}
