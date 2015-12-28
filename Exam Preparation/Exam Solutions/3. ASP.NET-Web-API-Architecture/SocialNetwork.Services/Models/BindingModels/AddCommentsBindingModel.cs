using System.ComponentModel.DataAnnotations;

namespace SocialNetwork.Services.Models.BindingModels
{
    public class AddCommentsBindingModel
    {
        [Required]
        [MinLength(5)]
        public string Content { get; set; }
    }
}