using System.ComponentModel.DataAnnotations;

namespace Messages.RestServices.Models.BindingModels
{
    public class MessageBindingModel
    {
        [Required]
        public string Text { get; set; }
    }
}