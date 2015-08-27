namespace TweeterLike.WebServices.Controllers
{
    using System.Linq;
    using System.Web.Http;
    using System.Web.OData;
    using Models.BindingModels;
    using Models.ViewModels;

    [Authorize]
    [RoutePrefix("api/user")]
    public class UserController : BaseApplicationController
    {
        // api/User?username={username}
        public IHttpActionResult GetUserInfo(string username)
        {
            var user = this.Data.ApplicationUsers.Find(u => u.UserName == username).FirstOrDefault();
            if (user == null)
            {
                return this.BadRequest(string.Format("User with username {0} not found!", username));
            }

            var userViewModel = new UserProfileViewModel()
            {
                Username = user.UserName,
                Email = user.Email,
                CreatedOn = user.CreatedOn,
                Posts = user.Posts.Select(p => p.Title)
            };

            return this.Ok(userViewModel);
        }

        // api/User?username={username}
        public IHttpActionResult DeleteUser(string username)
        {
            var user = this.Data.ApplicationUsers.Find(u => u.UserName == username).FirstOrDefault();
            if (user == null)
            {
                return this.BadRequest(string.Format("User with username {0} not found!", username));
            }

            this.Data.ApplicationUsers.Delete(user);
            this.Data.SaveChanges();

            return this.Ok();
        }


        //api/User?partialName={partialName}
        public IHttpActionResult GetSearchUsers(string partialName)
        {
            var users = this.Data.ApplicationUsers.Find(u => u.UserName.Contains(partialName)).Select(UserProfileViewModel.Create);
            
            if (!users.Any())
            {
                return this.Ok(new string [] {});
            }
   
            return this.Ok(users);
        }
    }
}
