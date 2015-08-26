namespace TweeterLike.WebServices.Models.ViewModels
{
    using System;
    using System.Linq.Expressions;
    using TweeterLike.Models.DbModels;

    public class PostViewModel
    {
        public string AuthorName { get; set; }

        public string Title { get; set; }

        public string Comment { get; set; }

        public DateTime CreateAt { get; set; }

        public static Expression<Func<Post, PostViewModel>> Create
        {
            get
            {
                return p => new PostViewModel()
                {
                    AuthorName = p.Author.UserName,
                    Title = p.Title,
                    Comment = p.Comment,
                    CreateAt = p.CreatedAt
                };
            }
        }
    }
}