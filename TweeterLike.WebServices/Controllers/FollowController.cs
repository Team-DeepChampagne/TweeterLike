namespace TweeterLike.WebServices.Controllers
{
    using System.Collections.Generic;
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
        public IHttpActionResult PostFollowUser([FromUri]FollowBindingModel model)
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
        public IHttpActionResult GetAllFollowedUsers(string username)
        {
            var followedUsersFiew = GetAllFollowedUsersEnumeration(username);
            if (followedUsersFiew == null)
            {
                return this.BadRequest("No such user");
            }

            return this.Ok(followedUsersFiew);
        }

        [Authorize]
        [Route("api/FollowedBy/Count")]
        public IHttpActionResult GetAllFollowedUsersCount(string username)
        {
            var followedUsersFiew = GetAllFollowedUsersEnumeration(username);
            if (followedUsersFiew == null)
            {
                return this.BadRequest("No such user");
            }

            return this.Ok(followedUsersFiew.Count());
        }

        [Authorize]
        [Route("api/Following")]
        public IHttpActionResult GetAllFollowingUsers(string username)
        {
            var followingUsersView = GetAllFollowingUsersEnumeration(username);
            if (followingUsersView == null)
            {
                return this.BadRequest("No such user");
            }

            return this.Ok(followingUsersView);
        }

        [Authorize]
        [Route("api/Following/Count")]
        public IHttpActionResult GetFollowingUsersCount(string username)
        {
            var followingUsersView = GetAllFollowingUsersEnumeration(username);
            if (followingUsersView == null)
            {
                return this.BadRequest("No such user");
            }

            return this.Ok(followingUsersView.Count());             
        }

        private IEnumerable<UserProfileViewModel> GetAllFollowingUsersEnumeration(string username){
            var followingUsers = this.Data.ApplicationUsers.Find(u => u.UserName == username).FirstOrDefault();

            if (followingUsers == null)
            {
                return null;
            }

            var followedView = followingUsers.Following
                .AsQueryable()
                .Select(UserProfileViewModel.Create);

            return followedView;
            
        }

        private IEnumerable<UserProfileViewModel> GetAllFollowedUsersEnumeration(string username)
        {
            var followedUsers = this.Data.ApplicationUsers.Find(u => u.UserName == username).FirstOrDefault();

            if (followedUsers == null)
            {
                return null;
            }

            var followedView = followedUsers.Followed
                .AsQueryable()
                .Select(UserProfileViewModel.Create);

            return followedView;
        }
    }
}
