using System;
using System.Collections.Generic;
using System.Data.Entity;
using Restauranteur.Models;
using Restaurants.Data.Repositories;
using Restaurants.Models;

namespace Restaurants.Data.UnitOfWork
{
    public class RestaurantsUnitOfWork : IRestaurantsUnitOfWork
    {
        private readonly DbContext dbContext;

        private readonly IDictionary<Type, object> repositories;

        public RestaurantsUnitOfWork()
            : this(new RestaurantsContext())
        {
        }

        public RestaurantsUnitOfWork(DbContext dbContext)
        {
            this.dbContext = dbContext;
            this.repositories = new Dictionary<Type, object>();
        }

        public IRepository<Meal> Meals
        {
            get { return this.GetRepository<Meal>(); }
        }

        public IRepository<MealType> MealTypes
        {
            get { return this.GetRepository<MealType>(); }
        }

        public IRepository<Order> Orders
        {
            get { return this.GetRepository<Order>(); }
        }

        public IRepository<Rating> Ratings
        {
            get { return this.GetRepository<Rating>(); }
        }

        public IRepository<Restaurant> Restaurants
        {
            get { return this.GetRepository<Restaurant>(); }
        }

        public IRepository<Town> Towns
        {
            get { return this.GetRepository<Town>(); }
        }

        public IRepository<ApplicationUser> ApplicationUsers
        {
            get { return this.GetRepository<ApplicationUser>(); }
        }

        public int SaveChanges()
        {
            return this.dbContext.SaveChanges();
        }

        private IRepository<T> GetRepository<T>() where T : class
        {
            if (!this.repositories.ContainsKey(typeof(T)))
            {
                var type = typeof(GenericEfRepository<T>);
                this.repositories.Add(typeof(T),
                    Activator.CreateInstance(type, this.dbContext));
            }

            return (IRepository<T>)this.repositories[typeof(T)];
        }
    }
}
