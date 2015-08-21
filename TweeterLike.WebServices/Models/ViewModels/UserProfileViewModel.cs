namespace TweeterLike.WebServices.Models.ViewModels
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using TweeterLike.Models.DbModels;

    public class UserProfileViewModel
    {
        public UserProfileViewModel(ApplicationUser user)
        {
            this.Username = user.UserName;
            this.Email = user.Email;
            this.CreatedOn = user.CreatedOn;
            this.Posts = user.Posts.Select(p => p.Title);
        }

        public string Username { get; set; }

        public string Email { get; set; }

        public DateTime CreatedOn { get; set; }

        public IEnumerable<string> Posts { get; set; }

        public static Expression<Func<ApplicationUser, UserProfileViewModel>> Create
        {
            get { return a => new UserProfileViewModel(a); }
        }
    }
}