using System.ComponentModel.DataAnnotations;

namespace Restaurants.Services.Models.BindingModels
{
    public class EditMealBindingModel
    {
        [Required]
        public string Name { get; set; }

        public decimal Price { get; set; }

        public int TypeId { get; set; }
    }
}