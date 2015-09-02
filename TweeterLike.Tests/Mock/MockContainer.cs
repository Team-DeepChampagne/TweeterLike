namespace TweeterLike.Tests.Mock
{
    using System.Collections.Generic;
    using System.Linq;
    using Data.Repositories;
    using Moq;
    using TweeterLike.Models.DbModels;

    public class MockContainer
    {
        public Mock<IRepository<ApplicationUser>> UsersRepositoryMock { get; set; }
        public Mock<IRepository<Post>> PostsRepositoryMock { get; set; }
        public Mock<IRepository<Reply>> RepliesRepositoryMock { get; set; }

        public MockContainer()
        {
            this.UsersRepositoryMock = new Mock<IRepository<ApplicationUser>>();
            this.PostsRepositoryMock = new Mock<IRepository<Post>>();
            this.RepliesRepositoryMock = new Mock<IRepository<Reply>>();
            this.SetupMocks();
        }

        private void SetupMocks()
        {
            this.SetupUsers();
            this.SetupPosts();
            this.SetupReplies();
        }

        private void SetupReplies()
        {
            var fakeReplies = new List<Reply>();

            // Post Delete test
            this.RepliesRepositoryMock.Setup(r => r.Delete(It.IsAny<Reply>()));
            this.RepliesRepositoryMock.Setup(r => r.Add(It.IsAny<Reply>()))
                .Callback((Reply r) =>
                {
                    fakeReplies.Add(r);
                });
            this.RepliesRepositoryMock.Setup(r => r.All()).Returns(fakeReplies.AsQueryable());
        }

        private void SetupPosts()
        {
            var fakePosts = new List<Post>();

            // Post Add test
            this.PostsRepositoryMock
                .Setup(p => p.Add(It.IsAny<Post>()))
                .Callback((Post p) =>
                {
                    fakePosts.Add(p);
                });
            this.PostsRepositoryMock
                .Setup(p => p.All())
                .Returns(fakePosts.AsQueryable());

            // Post Delete when not owner; Post Reply
            this.PostsRepositoryMock
                .Setup(p => p.GetById(It.Is<int>(id => id == 2)))
                .Returns(new Post()
                {
                    Author = new ApplicationUser() { Id = "2", UserName = "Some" },
                    Comment = "Others",
                    Title = "Others",
                    Id = 1,
                    Replies = new List<Reply>()
                });

            // Post Delete when owner; Get Replies; Post Reply
            this.PostsRepositoryMock
                .Setup(p => p.GetById(It.Is<int>(id => id == 1)))
                .Returns(new Post()
                {
                    Id = 1,
                    Author = new ApplicationUser() { UserName = "First", Id = "1" },
                    Comment = "First",
                    Title = "First Title",
                    Replies = new List<Reply>()
                    {
                        new Reply()
                        {
                            Id = 1,
                            Author = new ApplicationUser() {UserName = "Second", Id = "2"},
                            Comment = "First reply",
                            Post = new Post(){Id = 1}
                        }
                    }
                });
        }

        private void SetupUsers()
        {
            var firstUser = new ApplicationUser()
            {
                UserName = "First",
                Id = "1",
                Posts = new List<Post>()
                {
                    new Post()
                    {
                        Id = 1,
                        Author = new ApplicationUser() {UserName = "First", Id = "1"},
                        Comment = "First",
                        Title = "First Title",
                        Replies = new List<Reply>()
                        {
                            new Reply()
                            {
                                Id = 1,
                                Author = new ApplicationUser() {UserName = "Second", Id = "2"},
                                Comment = "First reply",
                                Post = new Post(){Id=1}
                            }
                        }
                    },
                    new Post()
                    {
                        Id = 2,
                        Author = new ApplicationUser() {UserName = "First", Id = "1"},
                        Comment = "Second",
                        Title = "Second Title",
                        Replies = new List<Reply>()
                    },
                    new Post()
                    {
                        Id = 3,
                        Author = new ApplicationUser() {UserName = "First", Id = "1"},
                        Comment = "Third",
                        Title = "Third Title",
                        Replies = new List<Reply>()
                    }
                },
                Following = new List<ApplicationUser>()
                {
                    new ApplicationUser()
                    {
                        UserName = "FirstFollowing",
                        Id = "1234",
                        Posts = new List<Post>()
                        {
                            new Post()
                            {
                                Id = 20,
                                Author = new ApplicationUser() {UserName = "First1", Id = "11"},
                                Comment = "Second1",
                                Title = "Second Title1",
                                Replies = new List<Reply>()
                            },
                            new Post()
                            {
                                Id = 21,
                                Author = new ApplicationUser() {UserName = "First2", Id = "12"},
                                Comment = "Second2",
                                Title = "Second Title2",
                                Replies = new List<Reply>()
                            }
                        }
                    }
                }
            };
            var fakeUsers = new List<ApplicationUser>()
            {
                firstUser
            };

            this.UsersRepositoryMock.Setup(p => p.All()).Returns(fakeUsers.AsQueryable());
            this.UsersRepositoryMock.Setup(p => p.GetById(It.IsAny<string>()))
                .Returns(firstUser);
        }
    }
}