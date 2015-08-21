namespace TweeterLike.WebServices.Controllers
{
    using System.Collections.Generic;
    using System.Web.Http;
    using Microsoft.AspNet.Identity;
    using Models.BindingModels;
    using Models.ViewModels;
    using TweeterLike.Models.DbModels;

    [RoutePrefix("api/post")]
    public class PostController : BaseApplicationController
    {
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

            var user = this.Data.ApplicationUsers.GetById(this.User.Identity.GetUserId());
            var post = new Post()
            {
                Title = model.Title,
                Comment = model.Comment,
                Replies = new List<Reply>(),
                Author = user
            };

            var postView = new PostViewModel()
            {
                AuthorName = post.Author.UserName,
                Title = post.Title,
                Comment = post.Comment,
                CreateAt = post.CreatedAt
            };

            return this.Ok(postView);
        }
    }
}
