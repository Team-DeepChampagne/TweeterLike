namespace TweeterLike.WebServices.Controllers
{
    using System.Web.Http;
    using Data.Context;
    using Data.DataLayer;
    using Infrastructure;

    public class BaseApplicationController : ApiController
    {
        public BaseApplicationController(ITweeterLikeData tweeterLikeData, IUserIdProvider idProvider)
        {
            this.Data = tweeterLikeData;
            this.UserIdProvider = idProvider;
        }

        public BaseApplicationController()
            : this(new TweeterLikeData(new TweeterLikeContext()), new AspNetUserIdProvider())
        {
        }

        protected ITweeterLikeData Data { get; set; }

        protected IUserIdProvider UserIdProvider { get; set; }
    }
}
