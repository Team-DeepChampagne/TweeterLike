namespace TweeterLike.WebServices.Controllers
{
    using System.Collections.Generic;
    using System.Web.Http;
    using Microsoft.AspNet.Identity;
    using Models.BindingModels;
    using Models.ViewModels;
    using TweeterLike.Models.DbModels;
    using System.Linq;
    using Data.DataLayer;
    using Infrastructure;

    [RoutePrefix("api/post")]
    public class PostController : BaseApplicationController
    {
        public PostController()
            : base()
        {
        }

        public PostController(ITweeterLikeData tweeterLikeData, IUserIdProvider idProvider)
            : base(tweeterLikeData, idProvider)
        {
        }

        // api/post
        [Authorize]
        public IHttpActionResult PostAddNewPost(NewPostBindingModel model)
        {
            if (model == null)
            {
                return this.BadRequest("Empty post is not allowed!");
            }

            if (!this.ModelState.IsValid)
            {
                return this.BadRequest(this.ModelState);
            }

            var user = this.Data.ApplicationUsers.GetById(this.UserIdProvider.GetUserId());
            var post = new Post()
            {
                Title = model.Title,
                Comment = model.Comment,
                Replies = new List<Reply>(),
                Author = user
            };

            this.Data.Posts.Add(post);
            this.Data.SaveChanges();

            var postView = new PostViewModel()
            {
                Id = post.Id,
                AuthorName = post.Author.UserName,
                Title = post.Title,
                Comment = post.Comment,
                CreateAt = post.CreatedAt,
            };
            return this.Ok(postView);
        }

        //GET api/post?username=username
        [Authorize]
        public IHttpActionResult GetAllPostsForUser(string username)
        {
            var user = this.Data.ApplicationUsers.All().FirstOrDefault(u => u.UserName == username);

            if (user == null)
            {
                return this.BadRequest(Messege.NoSuchUser);
            }

            var userPosts = user.Posts.OrderByDescending(p => p.CreatedAt).AsQueryable();
            var userPostsViewModel = userPosts.Select(PostViewModel.Create);

            if (!this.ModelState.IsValid)
            {
                return this.BadRequest(this.ModelState);
            }

            return this.Ok(userPostsViewModel);
        }

        [Authorize]
        public IHttpActionResult DeletePost(int id)
        {
            var post = this.Data.Posts.GetById(id);

            if (post == null)
            {
                return this.BadRequest(Messege.NoSuchPostError);
            }

            if (this.UserIdProvider.GetUserId() != post.Author.Id)
            {
                return this.BadRequest(Messege.NotYourPostError);
            }

            List<Reply> repliesToRemove = new List<Reply>();

            foreach (var reply in post.Replies)
            {
                repliesToRemove.Add(reply);
            }

            foreach (var replyToRemove in repliesToRemove)
            {
                this.Data.Replies.Delete(replyToRemove);
            }

            this.Data.Posts.Delete(post);
            this.Data.SaveChanges();

            return this.Ok();
        }

        [Authorize]
        public IHttpActionResult GetAllPostsForFollowingUsers()
        {
            var postsView = this.Data.ApplicationUsers
                .GetById(this.UserIdProvider.GetUserId())
                .Following
                .SelectMany(u => u.Posts)
                .OrderByDescending(u => u.CreatedAt)
                .AsQueryable()
                .Select(PostViewModel.Create);

            return this.Ok(postsView);
        }
    }
}