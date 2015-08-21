namespace TweeterLike.Data.Repositories
{
    using Context;
    using Models.DbModels;

    public class ReplyRepository : Repository<Reply>
    {
        public ReplyRepository(TweeterLikeContext context)
            : base(context)
        {
        }

        public override void Delete(Reply entity)
        {
            this.DbSet.Remove(entity);
        }
    }
}
