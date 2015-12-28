using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using OnlineShop.Models;

namespace OnlineShop.Services.Models.ViewModels
{
    public class AdViewModel
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public decimal Price { get; set; }

        public string Description { get; set; }

        public UserViewModel Owner { get; set; }

        public DateTime PostedOn { get; set; }

        public IEnumerable<CategoryViewModel> Categories { get; set; }

        public static Expression<Func<Ad, AdViewModel>> Create
        {
            get
            {
                return ad => new AdViewModel()
                {
                    Categories = ad.Categories
                        .Select(c => new CategoryViewModel()
                        {
                            Id = c.Id,
                            Name = c.Name
                        }),
                    Name = ad.Name,
                    Id = ad.Id,
                    Description = ad.Description,
                    PostedOn = ad.PostedOn,
                    Price = ad.Price,
                    //Owner = ad.Owner.UserName
                };
            }
        } 
    }
}