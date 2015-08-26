namespace TweeterLike.WebServices.Controllers
{
    using System.Collections.Generic;
    using System.Web.Http;
    using Microsoft.AspNet.Identity;
    using Models.BindingModels;
    using Models.ViewModels;
    using TweeterLike.Models.DbModels;
    using System.Linq;

    [RoutePrefix("api/reply")]
    public class ReplyController : BaseApplicationController
    {
        //POST api/reply?postId=
        [Authorize]
        public IHttpActionResult PostAddNewReply(int postId, NewReplyBindingModel model)
        {
            var post = this.Data.Posts.GetById(postId);

            if (post == null)
            {
                return this.BadRequest("No such post");
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
                return this.BadRequest("No such post");
            }

            var postReplies = post.Replies.OrderByDescending(r => r.CreatedAt).AsQueryable();
            var repliesView = postReplies.Select(ReplyViewModel.Create);

            return this.Ok(repliesView);
        }
    }
}
