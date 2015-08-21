namespace TweeterLike.Data.DataLayer
{
    using Models.DbModels;
    using Repositories;

    public interface ITweeterLikeData
    {
        ApplicationUserRepository ApplicationUsers { get; }

        PostRepository Posts { get; }

        ReplyRepository Replies { get; } 
    }
}