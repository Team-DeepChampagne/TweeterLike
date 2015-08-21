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

        public static TweeterLikeContext Create()
        {
            return new TweeterLikeContext();
        }
    }
}