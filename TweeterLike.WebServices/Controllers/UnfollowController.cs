namespace TweeterLike.WebServices.Controllers
{
    using System.Linq;
    using System.Web.Http;
    using Microsoft.AspNet.Identity;
    using Models.BindingModels;

    public class UnfollowController : BaseApplicationController
    {
        [Authorize]
        // api/Unfollow?Username={Username}
        public IHttpActionResult GetUnfollowUser([FromUri]FollowBindingModel model)
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
                return this.BadRequest("You cannot unfollow yourself!");
            }

            var userToFollow = this.Data.ApplicationUsers.Find(u => u.UserName == model.Username)
                .FirstOrDefault();

            if (userToFollow == null)
            {
                return this.BadRequest("No user with username " + model.Username);
            }

            if (!userToFollow.Followed.Contains(currentUser))
            {
                return this.BadRequest(userToFollow.UserName + " not followed!");
            }

            currentUser.Following.Remove(userToFollow);
            userToFollow.Followed.Remove(currentUser);
            this.Data.SaveChanges();

            return this.Ok(userToFollow.UserName + " unfollowed successfully!");
        }
    }
}
