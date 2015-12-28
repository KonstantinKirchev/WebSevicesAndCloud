using System.ComponentModel.DataAnnotations;

namespace Restaurants.Services.Models.BindingModels
{
    public class RestaurantBindingModel
    {
        public string Name { get; set; }

        [Required]
        public int TownId { get; set; }
    }
}