using System.ComponentModel.DataAnnotations;
using Messages.Data.Models;

namespace Messages.RestServices.Models.BindingModels
{
    public class UserMessageBindingModel
    {
        [Required]
        public string Text { get; set; }

        [Required]
        public string Recipient { get; set; }
    }
}