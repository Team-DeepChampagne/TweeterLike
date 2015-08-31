namespace TweeterLike.WebServices.Controllers
{
    using System;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Web.Http;
    using Data.DataLayer;
    using Infrastructure;
    using Microsoft.AspNet.Identity;
    using Models.BindingModels;
    using Models.ViewModels;
    using TweeterLike.Models.DbModels;

    [RoutePrefix("api/reply")]
    public class ReplyController : BaseApplicationController
    {
        public ReplyController()
            : base()
        {
        }

        public ReplyController(ITweeterLikeData tweeterLikeData, IUserIdProvider idProvider)
            : base(tweeterLikeData, idProvider)
        {
        }

        //POST api/reply?postId=
        [Authorize]
        public IHttpActionResult PostAddNewReply(int postId, NewReplyBindingModel model)
        {
            var post = this.Data.Posts.GetById(postId);

            if (post == null)
            {
                return this.BadRequest(Messege.NoSuchPostError);
            }

            if (model == null)
            {
                return this.BadRequest("Empty reply is not allowed!");
            }

            if (!this.ModelState.IsValid)
            {
                return this.BadRequest(this.ModelState);
            }

            var user = this.Data.ApplicationUsers.GetById(this.User.Identity.GetUserId());

            var reply = new Reply()
            {
                Author = user,
                Comment = model.Comment,
                Post = post
            };

            var replyView = new ReplyViewModel()
            {
                AuthorName = user.UserName,
                Comment = model.Comment,
                CreateAt = reply.CreatedAt,
                RepliedPostId = postId
            };


            post.Replies.Add(reply);
            this.Data.SaveChanges();
            return this.Ok(replyView);
        }

        //GET api/reply?postId=
        public IHttpActionResult GetRepliesForPost(int postId)
        {
            var post = this.Data.Posts.GetById(postId);

            if (post == null)
            {
                return this.BadRequest(Messege.NoSuchPostError);
            }

            var postReplies = post.Replies
                .OrderByDescending(r => r.CreatedAt)
                .AsQueryable();
            var repliesView = postReplies
                .Select(ReplyViewModel.Create)
                .ToList();

            return this.Ok(repliesView);
        }
    }
}
