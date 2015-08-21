namespace TweeterLike.Data.DataLayer
{
    using Context;
    using Repositories;

    public class TweeterLikeData : ITweeterLikeData
    {
        private ApplicationUserRepository applicationUserRepository;
        private PostRepository postRepository;
        private ReplyRepository replyRepository;

        public TweeterLikeData(TweeterLikeContext context)
        {
            this.Context = context;
        }

        public TweeterLikeContext Context { get; private set; }

        public ApplicationUserRepository ApplicationUsers
        {
            get
            {
                if (this.applicationUserRepository == null)
                {
                    this.applicationUserRepository = new ApplicationUserRepository(this.Context);
                }

                return this.applicationUserRepository;
            }
        }

        public PostRepository Posts
        {
            get
            {
                if (this.postRepository == null)
                {
                    this.postRepository = new PostRepository(this.Context);
                }

                return this.postRepository;
            }
        }

        public ReplyRepository Replies
        {
            get
            {
                if (this.replyRepository == null)
                {
                    this.replyRepository = new ReplyRepository(this.Context);
                }

                return this.replyRepository;
            }
        }
    }
}