using System.ComponentModel.DataAnnotations;

namespace Restaurants.Services.Models.BindingModels
{
    public class PendingOrderBindingModel
    {
        [Required]
        public string StartPage { get; set; }

        [Required]
        public string Limit { get; set; }

        public int? MealId { get; set; }
    }
}