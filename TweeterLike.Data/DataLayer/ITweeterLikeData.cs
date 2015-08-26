namespace TweeterLike.Data.DataLayer
{
    using Models.DbModels;
    using Repositories;

    public interface ITweeterLikeData
    {
        IRepository<ApplicationUser> ApplicationUsers { get; }

        IRepository<Post> Posts { get; }

        IRepository<Reply> Replies { get; }

        int SaveChanges();
    }
}