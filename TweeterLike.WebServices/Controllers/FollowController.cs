namespace TweeterLike.WebServices.Controllers
{
    using System.Linq;
    using System.Web.Http;
    using Microsoft.AspNet.Identity;
    using Models.BindingModels;

    public class FollowController : BaseApplicationController
    {
        [Authorize]
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

            if (userToFollow.Followers.Contains(currentUser))
            {
                return this.BadRequest(userToFollow.UserName + " already followed!");
            }

            userToFollow.Followers.Add(currentUser);
            this.Data.SaveChanges();

            return this.Ok(userToFollow.UserName + " followed successfully!");
        }
    }
}
