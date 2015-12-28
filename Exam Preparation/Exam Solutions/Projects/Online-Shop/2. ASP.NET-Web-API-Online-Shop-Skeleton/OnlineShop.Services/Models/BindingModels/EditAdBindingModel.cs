using System;
using System.ComponentModel.DataAnnotations;
using OnlineShop.Models;

namespace OnlineShop.Services.Models.BindingModels
{
    public class EditAdBindingModel
    {
        [Required]
        public AdStatus Status { get; set; }

        [Required]
        public DateTime ClosedOn { get; set; }
    }
}