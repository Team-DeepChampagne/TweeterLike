using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TweeterLike.WebServices.Models.BindingModels
{
    using System.ComponentModel.DataAnnotations;
    using TweeterLike.Models;

    public class NewPostBindingModel
    {
        [Required(ErrorMessage = Messeges.Required)]
        [StringLength(50, ErrorMessage = Messeges.MinLength, MinimumLength = 6)]
        [Display(Name = "Title")]
        public string Title { get; set; }

        [Required(ErrorMessage = Messeges.Required)]
        [StringLength(200, ErrorMessage = Messeges.MinLength, MinimumLength = 6)]
        [Display(Name = "Comment")]
        public string Comment { get; set; }
    }
}