namespace TweeterLike.Tests.Controllers
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Net;
    using System.Net.Http;
    using System.Threading;
    using System.Web.Http;
    using Data.DataLayer;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Mock;
    using Moq;
    using WebServices.Controllers;
    using WebServices.Infrastructure;
    using WebServices.Models.BindingModels;
    using WebServices.Models.JsonDeserializeModels;
    using WebServices.Models.ViewModels;

    [TestClass]
    public class PostControllerTests
    {
        private MockContainer mock;
        private PostController controller;

        [TestInitialize]
        public void SetupMock()
        {
            // Setup fake data
            this.mock = new MockContainer();
            var fakeUsers = this.mock.UsersRepositoryMock.Object;
            var fakePosts = this.mock.PostsRepositoryMock.Object;
            var fakeReplies = this.mock.RepliesRepositoryMock.Object;
            var context = new Mock<ITweeterLikeData>();

            // Setup repositories
            context.Setup(u => u.ApplicationUsers).Returns(fakeUsers);
            context.Setup(u => u.Posts).Returns(fakePosts);
            context.Setup(u => u.Replies).Returns(fakeReplies);

            // Setup idProvider
            var idProvider = new Mock<IUserIdProvider>();
            idProvider.Setup(p => p.GetUserId()).Returns("1");

            // Setup controller
            this.controller = new PostController(context.Object, idProvider.Object);
            this.controller.Request = new HttpRequestMessage();
            this.controller.Configuration = new HttpConfiguration();
        }

        [TestMethod]
        public void TestGetAllPostsForUserWhenNoSuchUserShouldReturnBadRequest()
        {
            var response = this.controller.GetAllPostsForUser("not existing")
                .ExecuteAsync(CancellationToken.None)
                .Result;
            Assert.AreEqual(HttpStatusCode.BadRequest, response.StatusCode);
            var error = response.Content.ReadAsAsync<JsonMessegeModel>().Result;
            Assert.AreEqual(Messege.NoSuchUser, error.Message);
        }

        [TestMethod]
        public void TestGetAllPostsForUserWhenUserExistsShouldReturnAllHisPosts()
        {
            var response = this.controller.GetAllPostsForUser("First")
                .ExecuteAsync(CancellationToken.None)
                .Result;

            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
            var posts = response.Content
                .ReadAsAsync<IEnumerable<PostViewModel>>()
                .Result
                .Select(p => p.Title)
                .ToList();
            var fakeUser = this.mock.UsersRepositoryMock.Object.All()
                .First(u => u.UserName == "First");
            var fakePosts = fakeUser.Posts
                .OrderByDescending(p => p.CreatedAt)
                .Select(p => p.Title)
                .ToList();
            CollectionAssert.AreEqual(fakePosts, posts);
        }

        [TestMethod]
        public void TestGetAllPostsForFollowingUsersShouldReturnAllPostsForTheFollowingUsers()
        {
            var response = this.controller.GetAllPostsForFollowingUsers()
                .ExecuteAsync(CancellationToken.None)
                .Result;

            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
            var posts = response.Content
                .ReadAsAsync<IEnumerable<PostViewModel>>()
                .Result
                .Select(p => p.Title)
                .ToList();
            var fakeUser = this.mock.UsersRepositoryMock.Object.All()
               .First(u => u.UserName == "First");
            var fakePosts = fakeUser.Following
                .SelectMany(u => u.Posts)
                .OrderByDescending(u => u.CreatedAt)
                .Select(p => p.Title)
                .ToList();
            CollectionAssert.AreEqual(fakePosts, posts);
        }

        [TestMethod]
        public void TestPostAddNewPostWhenNoPostExistShouldAddOnlyOnePost()
        {
            var response = this.controller.PostAddNewPost(new NewPostBindingModel()
            {
                Comment = "Test",
                Title = "Test",
            })
            .ExecuteAsync(CancellationToken.None).Result;

            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);

            var post = response.Content
                .ReadAsAsync<PostViewModel>()
                .Result;
            var fakePosts = this.mock.PostsRepositoryMock.Object
                .All()
                .Select(p => p.Title)
                .ToList();
            Assert.AreEqual(1, fakePosts.Count);
            Assert.AreEqual(post.Title, fakePosts[0]);
        }

        [TestMethod]
        public void TestDeletePostWhenNoSuchPostExistShouldReturnBadRequest()
        {
            var response = this.controller.DeletePost(12)
                .ExecuteAsync(CancellationToken.None).Result;
            Assert.AreEqual(HttpStatusCode.BadRequest, response.StatusCode);

            var error = response.Content.ReadAsAsync<JsonMessegeModel>().Result;
            Assert.AreEqual(Messege.NoSuchPostError, error.Message);
        }


        [TestMethod]
        public void TestDeletePostWhenNotPostsAuthorShouldReturnBadRequest()
        {
            var response = this.controller.DeletePost(2).ExecuteAsync(CancellationToken.None).Result;

            Assert.AreEqual(HttpStatusCode.BadRequest, response.StatusCode);
            var error = response.Content.ReadAsAsync<JsonMessegeModel>().Result;
            Assert.AreEqual(Messege.NotYourPostError, error.Message);
        }

        [TestMethod]
        public void TestDeletePostWhenPostsAuthorShouldReturnDeleteIt()
        {
            var response = this.controller.DeletePost(1).ExecuteAsync(CancellationToken.None).Result;

            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
            var posts = this.mock.PostsRepositoryMock.Object.All().ToList();
            Assert.AreEqual(0, posts.Count);
        }
    }
}
