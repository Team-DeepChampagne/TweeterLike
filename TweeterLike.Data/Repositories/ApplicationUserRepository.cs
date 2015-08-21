namespace TweeterLike.Data.Repositories
{
    using Context;
    using Models.DbModels;

    public class ApplicationUserRepository
        : Repository<ApplicationUser>
    {
        public ApplicationUserRepository(TweeterLikeContext context)
            : base(context)
        {
        }

        public override void Delete(ApplicationUser entity)
        {
            this.DbSet.Remove(entity);
        }
    }
}
