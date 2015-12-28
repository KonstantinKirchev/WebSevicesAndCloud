using Messages.Data.Repositories;
using Messages.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Messages.Data
{
    public class MessagesData : IMessagesData
    {
        private DbContext context;
        private IDictionary<Type, object> repositories;

        public MessagesData(DbContext context)
        {
            this.context = context;
            this.repositories = new Dictionary<Type, object>();
        }


        public UsersRepository Users
        {
            get { return (UsersRepository)this.GetRepository<User>(); }
        }

        public IRepository<Channel> Channels
        {
            get { return this.GetRepository<Channel>(); }
        }

        public IRepository<ChannelMessage> ChannelMessages
        {
            get { return this.GetRepository<ChannelMessage>(); }
        }

        public UserMessagesRepository UserMessages
        {
            get { return (UserMessagesRepository)this.GetRepository<UserMessage>(); }
        }

        public int SaveChanges()
        {
            return this.context.SaveChanges();
        }


        private IRepository<T> GetRepository<T>() where T : class
        {
            var type = typeof(T);
            if (!this.repositories.ContainsKey(type))
            {
                var typeOfRepository = typeof(GenericReposory<T>);
                if (type.IsAssignableFrom(typeof(User)))
                {
                    typeOfRepository = typeof(UsersRepository);
                }

                if (type.IsAssignableFrom(typeof(UserMessage)))
                {
                    typeOfRepository = typeof(UserMessagesRepository);
                }

                var repository = Activator.CreateInstance(typeOfRepository, this.context);

                this.repositories.Add(type, repository);
            }

            return (IRepository<T>)this.repositories[type];
        }
    }
}
