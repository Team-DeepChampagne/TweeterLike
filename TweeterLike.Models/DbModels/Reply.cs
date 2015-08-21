namespace TweeterLike.Models.DbModels
{
    using System;
    using System.ComponentModel.DataAnnotations;

    public class Reply
    {
        public Reply()
        {
            this.CreatedAt = DateTime.Now;
        }

        public int Id { get; set; }

        [Required(ErrorMessage = Messeges.Required)]
        [Display(Name = "Author")]
        public virtual ApplicationUser Author { get; set; }

        [Required(ErrorMessage = Messeges.Required)]
        [StringLength(200, ErrorMessage = Messeges.MinLength, MinimumLength = 6)]
        [Display(Name = "Comment")]
        public string Comment { get; set; }

        public virtual Post Post { get; set; }

        public DateTime CreatedAt { get; private set; }

        public bool IsDeleted { get; set; }
    }
}