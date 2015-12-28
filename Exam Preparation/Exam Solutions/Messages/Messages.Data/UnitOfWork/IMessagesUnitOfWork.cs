using Messages.Data.Models;
using Messages.Data.Repositories;

namespace Messages.Data.UnitOfWork
{
    public interface IMessagesUnitOfWork
    {
        IRepository<User> Users { get; }

        IRepository<Channel> Channels { get; }

        IRepository<ChannelMessage> ChannelMessages { get; }

        IRepository<UserMessage> UserMessages { get; }

        void SaveChanges();
    }
}
