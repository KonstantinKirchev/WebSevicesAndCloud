using System.ComponentModel.DataAnnotations;

namespace Messages.RestServices.Models.BindingModels
{
    public class ChannelBindingModel
    {
        [Required]
        [StringLength(200, MinimumLength = 1, ErrorMessage = "Channel name should be in range [{1}...{2}]")]
        public string Name { get; set; }
    }
}