﻿namespace TweeterLike.WebServices.Controllers
{
    using System.Collections.Generic;
    using System.Web.Http;
    using Microsoft.AspNet.Identity;
    using Models.BindingModels;
    using Models.ViewModels;
    using TweeterLike.Models.DbModels;
    using System.Linq;

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

            this.Data.Posts.Add(post);
            this.Data.SaveChanges();
            return this.Ok(postView);
        }

        //GET api/post?username=username
        [Authorize]
        public IHttpActionResult GetAllPostsForUser(string username)
        {
            var user = this.Data.ApplicationUsers.All().FirstOrDefault(u => u.UserName == username);

            if (user == null)
            {
                return this.BadRequest("No such user");
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
                return this.BadRequest("No such post");
            }

            if (this.User.Identity.Name != post.Author.UserName)
            {
                return this.BadRequest("Can't delete post that is not yours");
            }

            foreach (var reply in post.Replies)
            {
                this.Data.Replies.Delete(reply);
            }

            this.Data.Posts.Delete(post);
            this.Data.SaveChanges();

            return this.Ok();
        }
    }
}
