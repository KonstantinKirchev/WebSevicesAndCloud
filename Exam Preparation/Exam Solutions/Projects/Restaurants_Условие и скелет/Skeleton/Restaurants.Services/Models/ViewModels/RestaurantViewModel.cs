using System.Collections;
using System.Collections.Generic;
using Restaurants.Models;

namespace Restaurants.Services.Models.ViewModels
{
    public class RestaurantViewModel
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public Rating Rating { get; set; }

        public IEnumerable<TownViewModel> Towns { get; set; }
    }
}