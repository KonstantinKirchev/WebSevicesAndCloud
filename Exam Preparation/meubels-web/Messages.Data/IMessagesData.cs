using Messages.Data.Repositories;
using Messages.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Messages.Data
{
    public interface IMessagesData
    {
        UsersRepository Users { get; }

        IRepository<Channel> Channels { get; }

        IRepository<ChannelMessage> ChannelMessages { get; }

        UserMessagesRepository UserMessages { get; }

        int SaveChanges();
    }
}
