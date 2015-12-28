using System.ComponentModel.DataAnnotations;

namespace Restaurants.Services.Models.BindingModels
{
    public class RateAnExistingRestaurantBindingModel
    {
        [Required]
        public int Stars { get; set; }
    }
}