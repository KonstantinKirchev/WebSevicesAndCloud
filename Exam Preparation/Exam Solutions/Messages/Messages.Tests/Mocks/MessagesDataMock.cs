using Messages.Data.Models;
using Messages.Data.Repositories;
using Messages.Data.UnitOfWork;

namespace Messages.Tests.Mocks
{
    public class MessagesDataMock : IMessagesUnitOfWork
    {
        private GenericRepositoryMock<User> usersMock = new GenericRepositoryMock<User>();
        private GenericRepositoryMock<Channel> channelsMock = new GenericRepositoryMock<Channel>();
        private GenericRepositoryMock<ChannelMessage> channelMessagesMock = new GenericRepositoryMock<ChannelMessage>();
        private GenericRepositoryMock<UserMessage> userMessagesMock = new GenericRepositoryMock<UserMessage>();

        public bool ChangesSaved { get; set; }

        public IRepository<User> Users {
            get { return this.usersMock; }
        }

        public IRepository<Channel> Channels
        {
            get { return this.channelsMock; }
        }

        public IRepository<ChannelMessage> ChannelMessages
        {
            get { return this.channelMessagesMock; }
        }

        public IRepository<UserMessage> UserMessages
        {
            get { return this.userMessagesMock; }
        }

        public void SaveChanges()
        {
            this.ChangesSaved = true;
        }
    }
}
