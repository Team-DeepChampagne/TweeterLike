namespace TweeterLike.WebServices.Models.BindingModels
{
    using System.ComponentModel.DataAnnotations;
    using TweeterLike.Models;

    public class EditPostBindingModel
    {
        [StringLength(50, ErrorMessage = Messeges.MinLength, MinimumLength = 6)]
        public string Title { get; set; }

        [StringLength(200, ErrorMessage = Messeges.MinLength, MinimumLength = 6)]
        public string Comment { get; set; }
    }
}