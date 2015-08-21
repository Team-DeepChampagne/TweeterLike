namespace TweeterLike.Data.Initialization
{
    using System.Data.Entity;
    using Context;

    public class DbInitializer: DropCreateDatabaseIfModelChanges<TweeterLikeContext>
    {
        protected override void Seed(TweeterLikeContext context)
        {
            // TODO: seed roles and simple data.
            base.Seed(context);
        }
    }
}
