using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Antlr.Runtime.Misc;
using Microsoft.Ajax.Utilities;
using OnlineShop.Models;

namespace OnlineShop.Services.Models
{
    public class AdViewModel
    {
        public string Name { get; set; }

        public string Description { get; set; }

        public UserViewModel Owner { get; set; }

        public IEnumerable<CategoryViewModel> Categories { get; set; }

        public static Expression<Func<Ad, AdViewModel>> Create
        {
            get
            {
                return a => new AdViewModel()
                {
                    Categories = a.Categories
                    .Select(c => new CategoryViewModel()
                    {
                        Id = c.Id,
                        Name = c.Name
                    }),
                    Name = a.Name,
                    Description = a.Description
                };
            }
        }
    }
}