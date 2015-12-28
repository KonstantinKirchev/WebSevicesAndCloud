using System;
using System.ComponentModel.DataAnnotations;

namespace BidSystem.RestServices.Models.BindingModels
{
    public class OfferBindingModel
    {
        [Required]
        public string Title { get; set; }

        public string Description { get; set; }

        public decimal InitialPrice { get; set; }

        public DateTime ExpirationDateTime { get; set; }
    }
}