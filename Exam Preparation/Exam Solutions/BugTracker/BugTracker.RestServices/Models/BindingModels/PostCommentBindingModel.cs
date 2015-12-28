using System.ComponentModel.DataAnnotations;

namespace BugTracker.RestServices.Models.BindingModels
{
    public class PostCommentBindingModel
    {
        [Required]
        public string Text { get; set; }
    }
}