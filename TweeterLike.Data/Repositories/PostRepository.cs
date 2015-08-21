namespace TweeterLike.Data.Repositories
{
    using Context;
    using Models.DbModels;

    public class PostRepository: Repository<Post>
    {
        public PostRepository(TweeterLikeContext context) : base(context)
        {
        }

        public override void Delete(Post entity)
        {
            this.DbSet.Remove(entity);
        }
    }
}
