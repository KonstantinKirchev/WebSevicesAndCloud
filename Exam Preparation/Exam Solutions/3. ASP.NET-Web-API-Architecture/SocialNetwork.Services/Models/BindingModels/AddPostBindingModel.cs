using System.ComponentModel.DataAnnotations;

namespace SocialNetwork.Services.Models.BindingModels
{
    public class AddPostBindingModel
    {
        [Required]
        public string Content { get; set; }

        [Required]
        public string WallOwnerUsername { get; set; }
    }
}