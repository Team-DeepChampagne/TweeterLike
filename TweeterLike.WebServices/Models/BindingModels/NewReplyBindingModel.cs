using System.ComponentModel.DataAnnotations;
using TweeterLike.Models;
using TweeterLike.Models.DbModels;


namespace TweeterLike.WebServices.Models.BindingModels
{
    public class NewReplyBindingModel
    {
        [Required(ErrorMessage = Messeges.Required)]
        [StringLength(200, ErrorMessage = Messeges.MinLength, MinimumLength = 6)]
        [Display(Name = "Comment")]
        public string Comment { get; set; }
    }
}