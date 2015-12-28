using System;
using System.Collections.Generic;
using System.Data.Entity;
using BugTracker.Data.Models;
using BugTracker.Data.Repositories;

namespace BugTracker.Data.UnitOfWork
{
    public class BugsUnitOfWork : IBugsUnitOfWork
    {
        private readonly DbContext dbContext;

        private readonly IDictionary<Type, object> repositories;

        public BugsUnitOfWork()
            : this(new BugTrackerDbContext())
        {
        }

        public BugsUnitOfWork(DbContext dbContext)
        {
            this.dbContext = dbContext;
            this.repositories = new Dictionary<Type, object>();
        }

        public IRepository<User> Users
        {
            get { return this.GetRepository<User>(); }
        }

        public IRepository<Bug> Bugs
        {
            get { return this.GetRepository<Bug>(); }
        }

        public IRepository<Comment> Comments
        {
            get { return this.GetRepository<Comment>(); }
        }

        public int SaveChanges()
        {
            return this.dbContext.SaveChanges();
        }

        private IRepository<T> GetRepository<T>() where T : class
        {
            if (!this.repositories.ContainsKey(typeof(T)))
            {
                var type = typeof(GenericRepository<T>);
                this.repositories.Add(typeof(T),
                    Activator.CreateInstance(type, this.dbContext));
            }

            return (IRepository<T>)this.repositories[typeof(T)];
        }
    }
}
