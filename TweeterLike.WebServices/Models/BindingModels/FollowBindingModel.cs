using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TweeterLike.WebServices.Models.BindingModels
{
    using System.ComponentModel.DataAnnotations;

    public class FollowBindingModel
    {
        [Required]
        public string Username { get; set; }
    }
}