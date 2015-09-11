namespace TweeterLike.Tests.IntergrationTests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net;
    using System.Net.Http;
    using System.Web.Http;
    using Data.Context;
    using EntityFramework.Extensions;
    using Microsoft.AspNet.Identity.EntityFramework;
    using Microsoft.Owin.Testing;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Models;
    using Owin;
    using TweeterLike.Models.DbModels;
    using WebServices;
    using WebServices.Models.ViewModels;

    [TestClass]
    public class IntergrationTests
    {
        private const string Username = "testUser";
        private const string Password = "Abc123$";

        private static TestServer httpTestServer;
        private static HttpClient httpClient;
        private string accessToken;

        public string AccessToken
        {
            get
            {
                if (this.accessToken == null)
                {
                    var loginResponse = this.Login();
                    Assert.AreEqual(HttpStatusCode.OK, loginResponse.StatusCode);

                    var loginData = loginResponse.Content.ReadAsAsync<LoginDto>().Result;
                    Assert.IsNotNull(loginData.Access_Token);

                    this.accessToken = loginData.Access_Token;
                }

                return this.accessToken;
            }
        }

        [AssemblyInitialize]
        public static void AssemblyInit(TestContext context)
        {
            ClearDatabase();

            httpTestServer = TestServer.Create(appBuilder =>
            {
                var config = new HttpConfiguration();
                WebApiConfig.Register(config);
                var startup = new Startup();

                startup.Configuration(appBuilder);
                appBuilder.UseWebApi(config);
            });

            httpClient = httpTestServer.HttpClient;

            SeedDatabase();
        }

        [AssemblyCleanup]
        public static void AssemblyCleanup()
        {
            if (httpTestServer != null)
            {
                httpTestServer.Dispose();
            }

            ClearDatabase();
        }

        [TestMethod]
        public void TestReplyToTopicWhenTopicHasValidIdShouldAddReplyToPost()
        {
            var data = new FormUrlEncodedContent(new[]
            {
                new KeyValuePair<string, string>("comment", "firstComment"), 
            });

            var context = new TweeterLikeContext();
            var postId = context.Posts.First().Id;
            var response = this.ReplyToPost(postId, data);
            if (response.StatusCode != HttpStatusCode.OK)
            {
                Assert.Fail(response.Content.ReadAsStringAsync().Result);
            }

            var createdReply = response.Content.ReadAsAsync<ReplyViewModel>().Result;
            Assert.AreEqual("firstComment", createdReply.Comment);
            Assert.AreEqual(Username, createdReply.AuthorName);
            Assert.AreEqual(postId, createdReply.RepliedPostId);
        }

        [TestMethod]
        public void TestReplyToTopicWhenTopicDonthaveValidIdShouldReturnBadRequest()
        {
            var data = new FormUrlEncodedContent(new[]
            {
                new KeyValuePair<string, string>("comment", "InvalidComment#1"), 
            });

            var invalidId = 100000;
            var context = new TweeterLikeContext();
            var dbPost = context.Posts.FirstOrDefault(p => p.Id == invalidId);
            Assert.IsNull(dbPost);

            var response = this.ReplyToPost(invalidId, data);
            Assert.AreEqual(HttpStatusCode.BadRequest, response.StatusCode);

            var dbReply = context.Replies.FirstOrDefault(r => r.Comment == "InvalidComment#1");
            Assert.IsNull(dbReply);
        }

        [TestMethod]
        public void TestReplyToTopicWhenTopicHasValidIdButReplyDataIsInvalidShouldReturnBadRequest()
        {
            var data = new FormUrlEncodedContent(new[]
            {
                new KeyValuePair<string, string>("cont", "InvalidComment#2"), 
            });

            var context = new TweeterLikeContext();
            var postId = context.Posts.First().Id;
            var response = this.ReplyToPost(postId, data);
            Assert.AreEqual(HttpStatusCode.BadRequest, response.StatusCode);

            var dbReply = context.Replies.FirstOrDefault(r => r.Comment == "InvalidComment#2");
            Assert.IsNull(dbReply);
        }

        [TestMethod]
        public void TestGetAllRepliesForTopicWhenTopicHasValidIdAndHasRepliesShouldReturnAllReplies()
        {
            var context = new TweeterLikeContext();
            var postId = context.Posts.First(p => p.Title == "PostWithReplies").Id;
            var response = this.GetRepliesForPost(postId);
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);

            var replies = response.Content.ReadAsAsync<IEnumerable<ReplyViewModel>>()
                .Result
                .Select(r => r.Comment)
                .ToList();

            var dbReplies = context.Posts.First(p => p.Id == postId).Replies
                .Select(r => r.Comment)
                .ToList();
            CollectionAssert.AreEqual(dbReplies, replies);
        }

        [TestMethod]
        public void TestGetAllRepliesForTopicWhenTopicHasValidIdAndDontHasAnyRepliesShouldReturnOkAndNoneReplies()
        {
            var context = new TweeterLikeContext();
            var postId = context.Posts.First(p => p.Title == "PostWithoutReplies").Id;
            var response = this.GetRepliesForPost(postId);
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);

            var replies = response.Content.ReadAsAsync<IEnumerable<ReplyViewModel>>()
                .Result
                .Select(r => r.Comment)
                .ToList();

            var dbReplies = context.Posts.First(p => p.Id == postId).Replies
                .Select(r => r.Comment)
                .ToList();
            CollectionAssert.AreEqual(dbReplies, replies);
            Assert.IsTrue(replies.Count != 0);
        }

        [TestMethod]
        public void TestGetAllRepliesForTopicWhenTopicHasInvalidIdShouldReturnBadRequest()
        {
            var invalidId = 100000;
            var context = new TweeterLikeContext();
            var dbPost = context.Posts.FirstOrDefault(p => p.Id == invalidId);
            Assert.IsNull(dbPost);

            var response = this.GetRepliesForPost(invalidId);
            Assert.AreEqual(HttpStatusCode.BadRequest, response.StatusCode);
        }

        private static void SeedDatabase()
        {
            var context = new TweeterLikeContext();
            var userStore = new UserStore<ApplicationUser>(context);
            var userManager = new ApplicationUserManager(userStore);

            var user = new ApplicationUser()
            {
                UserName = Username,
                Email = Username
            };

            var result = userManager.CreateAsync(user, Password).Result;
            if (!result.Succeeded)
            {
                Assert.Fail(string.Join(Environment.NewLine, result.Errors));
            }

            SeedPosts(context);
        }

        private static void ClearDatabase()
        {
            var context = new TweeterLikeContext();
            context.Replies.Delete();
            context.Posts.Delete();
            context.Users.Delete();
            context.SaveChanges();
        }

        private static void SeedPosts(TweeterLikeContext context)
        {
            context.Posts.Add(new Post()
            {
                Title = "PostWithoutReplies",
                Comment = "PostWithoutReplies",
                Replies = new List<Reply>(),
                Author = new ApplicationUser() { UserName = "SeedSeed", Id = "SeedSeed" }
            });
            context.Posts.Add(new Post()
            {
                Title = "PostWithReplies",
                Comment = "PostWithReplies",
                Replies = new List<Reply>()
                {
                    new Reply()
                    {
                        Author = new ApplicationUser() {UserName = "SeedSeed2", Id = "SeedSeed2"},
                        Comment = "FirstReply"
                    },
                    new Reply()
                    {
                        Author = new ApplicationUser() {UserName = "SeedSeed3", Id = "SeedSeed3"},
                        Comment = "SecondReply"
                    },
                },
                Author = new ApplicationUser() {UserName = "SeedSeed4", Id = "SeedSeed4"}
            });

            context.SaveChanges();
        }

        private HttpResponseMessage Login()
        {
            var loginData = new FormUrlEncodedContent(new[]
            {
                new KeyValuePair<string, string>("username", Username), 
                new KeyValuePair<string, string>("password", Password), 
                new KeyValuePair<string, string>("grant_type", "password"), 
            });

            var response = httpClient.PostAsync("/Token", loginData).Result;

            return response;
        }

        private void AddAuthenticationToken()
        {
            if (!httpClient.DefaultRequestHeaders.Contains("Authorization"))
            {
                httpClient.DefaultRequestHeaders
                    .Add("Authorization", "Bearer " + this.AccessToken);
            }
        }

        private void RemoveAuthenticatioToken()
        {
            if (httpClient.DefaultRequestHeaders.Contains("Authorization"))
            {
                httpClient.DefaultRequestHeaders
                    .Remove("Authorization");
            }
        }

        private HttpResponseMessage ReplyToPost(int id, FormUrlEncodedContent data)
        {
            this.AddAuthenticationToken();

            return httpClient.PostAsync("api/Reply?postId=" + id, data).Result;
        }

        private HttpResponseMessage GetRepliesForPost(int id)
        {
            this.RemoveAuthenticatioToken();

            return httpClient.GetAsync("api/Reply?postId=" + id + "&skip=0&take=100").Result;
        }
    }
}
