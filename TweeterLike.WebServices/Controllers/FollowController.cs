namespace TweeterLike.WebServices.Controllers
{
    using System.Linq;
    using System.Web.Http;
    using System.Web.Routing;
    using Microsoft.AspNet.Identity;
    using Models.BindingModels;
    using Models.ViewModels;

    public class FollowController : BaseApplicationController
    {
        [Authorize]
        // api/Follow?Username={Username}
        public IHttpActionResult GetFollowUser([FromUri]FollowBindingModel model)
        {
            if (model == null)
            {
                return this.BadRequest("Empty model is not allowed!");
            }

            if (!this.ModelState.IsValid)
            {
                return this.BadRequest(this.ModelState);
            }

            var currentUser = this.Data.ApplicationUsers.GetById(this.User.Identity.GetUserId());
            if (currentUser.UserName.ToLower() == model.Username.ToLower())
            {
                return this.BadRequest("You cannot follow yourself!");
            }

            var userToFollow = this.Data.ApplicationUsers.Find(u => u.UserName == model.Username)
                .FirstOrDefault();

            if (userToFollow == null)
            {
                return this.BadRequest("No user with username " + model.Username);
            }

            if (userToFollow.Followed.Contains(currentUser))
            {
                return this.BadRequest(userToFollow.UserName + " already followed!");
            }

            currentUser.Following.Add(userToFollow);
            userToFollow.Followed.Add(currentUser);
            this.Data.SaveChanges();

            return this.Ok(userToFollow.UserName + " followed successfully!");
        }

        [Authorize]
        [Route("api/FollowedBy")]
        public IHttpActionResult GetAllFollowedUsers()
        {
            var followedUsers = this.Data.ApplicationUsers
                .GetById(this.User.Identity.GetUserId());

            var followedView = followedUsers.Followed
                .Select(u => UserProfileViewModel.Create);

            return this.Ok(followedView);
        }

        [Authorize]
        [Route("api/Following")]
        public IHttpActionResult GetAllFollowingUsers()
        {
            var followedUsers = this.Data.ApplicationUsers
                .GetById(this.User.Identity.GetUserId());

            // For some reason using Expression breaks it.
            var followedView = followedUsers.Following
                .Select(u => new UserProfileViewModel()
                {
                    Posts = u.Posts.Select(p => p.Title),
                    Username = u.UserName,
                    CreatedOn = u.CreatedOn,
                    Email = u.Email
                });

            return this.Ok(followedView);
        }
    }
}
