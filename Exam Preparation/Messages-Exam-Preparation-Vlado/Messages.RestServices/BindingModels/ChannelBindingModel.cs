using System.ComponentModel.DataAnnotations;
namespace MessagesRestService.BindingModels
{
    public class ChannelBindingModel
    {
        [Required(ErrorMessage = "The name is required!")]
        [StringLength(100, MinimumLength = 3, 
            ErrorMessage = "The name should be between 3 and 100 symbols.")]
        public string Name { get; set; }
    }
}