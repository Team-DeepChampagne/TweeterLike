namespace TweeterLike.Data.DataLayer
{
    using System;
    using System.Collections.Generic;
    using Context;
    using Repositories;
    using Models.DbModels;

    public class TweeterLikeData : ITweeterLikeData
    {
        private readonly IDictionary<Type, object> repositories;

        public TweeterLikeData(TweeterLikeContext context)
        {
            this.Context = context;
            this.repositories = new Dictionary<Type, object>();
        }

        public TweeterLikeContext Context { get; private set; }

        public IRepository<ApplicationUser> ApplicationUsers
        {
            get { return this.GetRepository<ApplicationUser>(); }
        }

        public IRepository<Post> Posts
        {
            get { return this.GetRepository<Post>(); }
        }

        public IRepository<Reply> Replies
        {
            get { return this.GetRepository<Reply>(); }
        }

        public int SaveChanges()
        {
            return this.Context.SaveChanges();
        }

        private IRepository<T> GetRepository<T>() where T : class
        {
            var modelType = typeof(T);
            if (!this.repositories.ContainsKey(modelType))
            {
                var repositoryType = typeof(Repository<T>);
                this.repositories.Add(modelType, Activator.CreateInstance(repositoryType, this.Context));
            }

            return (IRepository<T>)this.repositories[modelType];
        }
    }
}
