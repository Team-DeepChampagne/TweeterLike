﻿namespace TweeterLike.Models.DbModels
{
    using System;
    using System.Collections.Generic;
    using System.Security.Claims;
    using System.Threading.Tasks;
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;

    public class ApplicationUser : IdentityUser
    {
        private ICollection<Post> posts;
        private ICollection<ApplicationUser> followed;
        private ICollection<ApplicationUser> following;

        public ApplicationUser()
        {
            this.posts = new HashSet<Post>();
            this.followed = new HashSet<ApplicationUser>();
            this.following = new HashSet<ApplicationUser>();
            this.CreatedOn = DateTime.Now;
        }

        public virtual ICollection<Post> Posts
        {
            get { return this.posts; }
            set { this.posts = value; }
        }

        public virtual ICollection<ApplicationUser> Followed
        {
            get { return this.followed; }
            set { this.followed = value; }
        }

        public virtual ICollection<ApplicationUser> Following
        {
            get { return this.following; }
            set { this.following = value; }
        }

        public DateTime CreatedOn { get; set; }

        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager,
            string authenticationType)
        {
            var userIdentity = await manager.CreateIdentityAsync(this, authenticationType);

            return userIdentity;
        }
    }
}
