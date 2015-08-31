using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace TweeterLike.Tests.Controllers
{
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net;
    using System.Net.Http;
    using System.Threading;
    using System.Web.Http;
    using Data.DataLayer;
    using Mock;
    using Moq;
    using WebServices.Controllers;
    using WebServices.Infrastructure;
    using WebServices.Models.BindingModels;
    using WebServices.Models.JsonDeserializeModels;
    using WebServices.Models.ViewModels;

    [TestClass]
    public class ReplyControllerTests
    {
        private MockContainer mock;
        private ReplyController controller;

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
            this.controller = new ReplyController(context.Object, idProvider.Object);
            this.controller.Request = new HttpRequestMessage();
            this.controller.Configuration = new HttpConfiguration();
        }

        [TestMethod]
        public void TestGetRepliesForPostWhenNoSuchPostShouldReturnBadRequest()
        {
            var response = this.controller.GetRepliesForPost(100).ExecuteAsync(CancellationToken.None).Result;
            Assert.AreEqual(HttpStatusCode.BadRequest, response.StatusCode);
            var error = response.Content.ReadAsAsync<JsonMessegeModel>().Result;
            Assert.AreEqual(Messege.NoSuchPostError, error.Message);
        }

        [TestMethod]
        public void TestGetRepliesForPostWhenNoRepliesForPostShouldReturnOkAndNoReplies()
        {
            var response = this.controller.GetRepliesForPost(2).ExecuteAsync(CancellationToken.None).Result;
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
            var replies = response.Content.ReadAsAsync<IEnumerable<ReplyViewModel>>().Result.ToList();
            Assert.AreEqual(0, replies.Count);
        }


        [TestMethod]
        public void TestGetRepliesForPostWhenHasRepliesForPostShouldReturnOkAndReplies()
        {
            var response = this.controller.GetRepliesForPost(1)
                .ExecuteAsync(CancellationToken.None)
                .Result;
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
            var replies = response.Content
                .ReadAsAsync<IEnumerable<ReplyViewModel>>()
                .Result
                .Select(r => r.Comment)
                .ToList();

            var fakeReplies = this.mock.PostsRepositoryMock.Object
                .GetById(1)
                .Replies
                .Select(r => r.Comment)
                .ToList();
            CollectionAssert.AreEqual(fakeReplies, replies);
        }

        [TestMethod]
        public void TestPostAddNewReplyWhenNoPostWithThatIdExistShouldReturnBadRequest()
        {
            var response = this.controller.PostAddNewReply(100, new NewReplyBindingModel{Comment = "Test"})
                .ExecuteAsync(CancellationToken.None)
                .Result;
            Assert.AreEqual(HttpStatusCode.BadRequest, response.StatusCode);
            var error = response.Content.ReadAsAsync<JsonMessegeModel>().Result;
            Assert.AreEqual(Messege.NoSuchPostError, error.Message);
        }

        [TestMethod]
        public void TestPostAddNewReplyWhenPostWithThatIdAndNoOtherRepliesExistShouldReturnOkAndAddReplyProperly()
        {
            var response = this.controller.PostAddNewReply(2, new NewReplyBindingModel { Comment = "Test" })
                .ExecuteAsync(CancellationToken.None)
                .Result;
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
            var replies = response.Content.ReadAsAsync<ReplyViewModel>()
                .Result;
            var fakeReplies = this.mock.PostsRepositoryMock.Object.GetById(2)
                .Replies
                .Select(r => r.Comment)
                .ToList();
            Assert.AreEqual(fakeReplies[0], replies.Comment);
        }

        [TestMethod]
        public void TestPostAddNewReplyWhenPostWithThatIdAndHasOtherRepliesExistShouldReturnOkAndAddReplyProperly()
        {
            var response = this.controller.PostAddNewReply(1, new NewReplyBindingModel {Comment = "Test"})
                .ExecuteAsync(CancellationToken.None)
                .Result;
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
            var replies = response.Content.ReadAsAsync<ReplyViewModel>()
                .Result;
            var fakeReplies = this.mock.PostsRepositoryMock.Object.GetById(1)
                .Replies
                .Select(r => r.Comment)
                .ToList();
            Assert.IsTrue(fakeReplies.Contains(replies.Comment));
        }
    }
}
