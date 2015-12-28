using System.ComponentModel.DataAnnotations;
namespace MessagesRestService.BindingModels
{
    public class ChannelMessageBindingModel
    {
        [Required(ErrorMessage = "The text is required!")]
        public string Text { get; set; }
    }
}