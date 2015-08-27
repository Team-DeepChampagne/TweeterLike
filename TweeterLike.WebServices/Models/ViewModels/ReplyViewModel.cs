using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using TweeterLike.Models.DbModels;

namespace TweeterLike.WebServices.Models.ViewModels
{
    public class ReplyViewModel
    {
        public int Id { get; set; }

        public string AuthorName { get; set; }

        public string Comment { get; set; }

        public int RepliedPostId { get; set; }

        public DateTime CreateAt { get; set; }

        public static Expression<Func<Reply, ReplyViewModel>> Create
        {
            get
            {
                return r => new ReplyViewModel()
                {
                    Id = r.Id,
                    AuthorName = r.Author.UserName,
                    Comment = r.Comment,
                    CreateAt = r.CreatedAt,
                    RepliedPostId = r.Post.Id
                };
            }
        }
    }
}