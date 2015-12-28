using System;
using System.Collections.Generic;
using System.Data.Entity;
using BidSystem.Data.Models;
using BidSystem.Data.Repositories;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace BidSystem.Data.UnitOfWork
{
    public class BidSystemData : IBidSystemData
    {
        private readonly DbContext context;
        private readonly IDictionary<Type, object> repositories;
        private IUserStore<User> userStore;

        public BidSystemData()
            : this(new BidSystemDbContext())
        {
            
        }

        public BidSystemData(DbContext context)
        {
            this.context = context;
            this.repositories = new Dictionary<Type, object>();
        }

        public IRepository<User> Users
        {
            get { return this.GetRepository<User>(); }
        }

        public IRepository<Offer> Offers
        {
            get { return this.GetRepository<Offer>(); }
        }

        public IRepository<Bid> Bids
        {
            get { return this.GetRepository<Bid>(); }
        }

        public IUserStore<User> UserStore
        {
            get
            {
                if (this.userStore == null)
                {
                    this.userStore = new UserStore<User>(this.context);
                }
                return this.userStore;
            }
        }
        
        public void SaveChanges()
        {
            this.context.SaveChanges();
        }

        private IRepository<T> GetRepository<T>() where T : class
        {
            if (!this.repositories.ContainsKey(typeof(T)))
            {
                var type = typeof(GenericRepository<T>);
                this.repositories.Add(typeof(T),
                    Activator.CreateInstance(type, this.context));
            }

            return (IRepository<T>)this.repositories[typeof(T)];
        }
    }
}
