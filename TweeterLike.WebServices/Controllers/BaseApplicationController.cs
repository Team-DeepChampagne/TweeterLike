namespace TweeterLike.WebServices.Controllers
{
    using System.Web.Http;
    using Data.Context;
    using Data.DataLayer;

    public class BaseApplicationController : ApiController
    {
        public BaseApplicationController(ITweeterLikeData tweeterLikeData)
        {
            this.Data = tweeterLikeData;
        }

        public BaseApplicationController()
            : this(new TweeterLikeData(new TweeterLikeContext()))
        {

        }

        protected ITweeterLikeData Data { get; set; }
    }
}
