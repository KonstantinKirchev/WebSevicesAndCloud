using System;
using System.Collections.Generic;
using System.Linq;
using Moq;
using OnlineShop.Data.Repositories;
using OnlineShop.Models;

namespace OnlineShop.Tests
{
    public class MockContainer
    {
        public Mock<IRepository<Ad>> AdRepositoryMock { get; set; }

        public Mock<IRepository<Category>> CategoriesRepositoryMock { get; set; }

        public Mock<IRepository<AdType>> AdTypeRepositoryMock { get; set; }

        public Mock<IRepository<ApplicationUser>> UserRepositoryMock { get; set; }

        public void PrepareMocks()
        {
            this.SetupFakeCategories();

            this.SetupFakeAds();

            this.SetupFakeAdTypes();

            this.SetupFakeUsers();
        }

        private void SetupFakeUsers()
        {
            var fakeUsers = new List<ApplicationUser>()
            {
                new ApplicationUser(){UserName = "blueeagle"},
                new ApplicationUser(){UserName = "peshkata"},
                new ApplicationUser(){UserName = "marijka"}
            };

            this.UserRepositoryMock = new Mock<IRepository<ApplicationUser>>();

            this.UserRepositoryMock.Setup(r => r.All()).Returns(fakeUsers.AsQueryable());

            this.UserRepositoryMock.Setup(r => r.Find(It.IsAny<int>()))
                .Returns((string id) =>
                {
                    return fakeUsers.FirstOrDefault(a => a.Id == id);
                });
        }

        private void SetupFakeAdTypes()
        {
            var fakeAdTypes = new List<AdType>()
            {
                new AdType(){Name = "Normal" , PricePerDay = 10000},
                new AdType(){Name = "Premium", PricePerDay = 5000}
            };

            this.AdTypeRepositoryMock = new Mock<IRepository<AdType>>();

            this.AdTypeRepositoryMock.Setup(r => r.All()).Returns(fakeAdTypes.AsQueryable());

            this.AdTypeRepositoryMock.Setup(r => r.Find(It.IsAny<int>()))
                .Returns((int id) =>
                {
                    return fakeAdTypes.FirstOrDefault(a => a.Id == id);
                });
        }


        private void SetupFakeCategories()
        {
            var fakeCategories = new List<Category>()
            {
                new Category(){Name = "Cars"},
                new Category(){Name = "Houses"},
                new Category(){Name = "Bykes"},
                new Category(){Name = "Clothes"}
            };

            this.CategoriesRepositoryMock = new Mock<IRepository<Category>>();

            this.CategoriesRepositoryMock.Setup(r => r.All()).Returns(fakeCategories.AsQueryable());

            this.CategoriesRepositoryMock.Setup(r => r.Find(It.IsAny<int>()))
                .Returns((int id) =>
                {
                    return fakeCategories.FirstOrDefault(a => a.Id == id);
                });
        }

        private void SetupFakeAds()
        {
            var adTypes = new List<AdType>()
            {
                new AdType(){Name = "Normal", Index = 100},
                new AdType(){Name = "Premium", Index = 200}
            };

            var fakeAds = new List<Ad>()
            {
                new Ad()
                {
                    Id = 5,
                    Name = "Audi A6",
                    Type = adTypes[0],
                    PostedOn = DateTime.Now.AddDays(-6),
                    Owner = new ApplicationUser(){UserName = "gosho", Id = "123"},
                    Price = 400
                },
                new Ad()
                {
                    Id = 6,
                    Name = "Audi A8",
                    Type = adTypes[1],
                    PostedOn = DateTime.Now.AddDays(-3),
                    Owner = new ApplicationUser(){UserName = "pesho", Id = "2121"},
                    Price = 800
                },
                new Ad()
                {
                    Id = 7,
                    Name = "Audi A4",
                    Type = adTypes[0],
                    PostedOn = DateTime.Now.AddDays(-1),
                    Owner = new ApplicationUser(){UserName = "kosta", Id = "1212"},
                    Price = 1000
                }
            };

            this.AdRepositoryMock = new Mock<IRepository<Ad>>();
            
            this.AdRepositoryMock.Setup(r => r.All()).Returns(fakeAds.AsQueryable());
            
            this.AdRepositoryMock.Setup(r => r.Find(It.IsAny<int>()))
                .Returns((int id) =>
                {
                    return fakeAds.FirstOrDefault(a => a.Id == id);
                });
        }
    }
}
