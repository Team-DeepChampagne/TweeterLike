namespace TweeterLike.Models.DbModels
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class Post
    {
        private ICollection<Reply> replies;

        public Post()
        {
            this.replies = new HashSet<Reply>();
            this.CreatedAt = DateTime.Now;
        }

        public int Id { get; set; }

        [Required(ErrorMessage = Messeges.Required)]
        [Display(Name = "Author")]
        public virtual ApplicationUser Author { get; set; }

        [Required(ErrorMessage = Messeges.Required)]
        [StringLength(50, ErrorMessage = Messeges.MinLength, MinimumLength = 6)]
        [Display(Name = "Title")]
        public string Title { get; set; }

        [Required(ErrorMessage = Messeges.Required)]
        [StringLength(200, ErrorMessage = Messeges.MinLength, MinimumLength = 6)]
        [Display(Name = "Comment")]
        public string Comment { get; set; }

        public virtual ICollection<Reply> Replies
        {
            get { return this.replies; }
            set { this.replies = value; }
        }

        public DateTime CreatedAt { get; private set; }

        public bool IsDeleted { get; set; }
    }
}
