namespace TweeterLike.Data.Context
{
    using System;
    using System.Data.Entity;
    using Initialization;
    using Microsoft.AspNet.Identity.EntityFramework;
    using Models.DbModels;

    public class TweeterLikeContext : IdentityDbContext<ApplicationUser>
    {
        public TweeterLikeContext()
            : base("name=TweeterLikeContext")
        {
            Database.SetInitializer(new DbInitializer());
        }

        public virtual DbSet<Post> Posts { get; set; }

        public virtual DbSet<Reply> Replies { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ApplicationUser>().Map(m => m.Requires("IsDeleted").HasValue(false)).Ignore(m => m.IsDeleted);
            modelBuilder.Entity<Post>().Map(m => m.Requires("IsDeleted").HasValue(false)).Ignore(m => m.IsDeleted);
            modelBuilder.Entity<Reply>().Map(m => m.Requires("IsDeleted").HasValue(false)).Ignore(m => m.IsDeleted);

            base.OnModelCreating(modelBuilder);
        }

        public static TweeterLikeContext Create()
        {
            return new TweeterLikeContext();
        }
    }
}