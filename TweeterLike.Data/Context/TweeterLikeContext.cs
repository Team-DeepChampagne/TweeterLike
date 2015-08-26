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
            modelBuilder.Entity<ApplicationUser>().HasMany(u => u.Following).WithMany()
            .Map(m =>
            {
                m.MapLeftKey("UserId");
                m.MapRightKey("FollowerId");
                m.ToTable("UsersFollowering");
            });

            modelBuilder.Entity<ApplicationUser>().HasMany(u => u.Followed).WithMany()
                .Map(m =>
                {
                    m.MapLeftKey("UserId");
                    m.MapRightKey("FollowedUserId");
                    m.ToTable("UsersFollowered");
                });

            base.OnModelCreating(modelBuilder);
        }

        public static TweeterLikeContext Create()
        {
            return new TweeterLikeContext();
        }
    }
}