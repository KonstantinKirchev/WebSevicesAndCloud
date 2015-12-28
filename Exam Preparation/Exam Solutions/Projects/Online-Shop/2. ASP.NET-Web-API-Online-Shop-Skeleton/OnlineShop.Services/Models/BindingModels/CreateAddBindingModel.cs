using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace OnlineShop.Services.Models.BindingModels
{
    public class CreateAddBindingModel
    {
        [Required]
        public string Name { get; set; }

        [Required]
        public string Description { get; set; }

        [Required]
        public int TypeId { get; set; }

        [Required]
        public decimal Price { get; set; }

        public IEnumerable<int> Categories { get; set; }
    }
}