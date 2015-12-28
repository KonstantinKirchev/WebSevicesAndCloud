using BidSystem.Data.Models;
using BidSystem.Data.Repositories;
using BidSystem.Data.UnitOfWork;
using Microsoft.AspNet.Identity;

namespace BidSystem.Tests.Mocks
{
    public class BidSystemDataMock : IBidSystemData
    {
        private GenericRepositoryMock<User> usersMock = new GenericRepositoryMock<User>();
        private GenericRepositoryMock<Bid> bidsMock = new GenericRepositoryMock<Bid>();
        private GenericRepositoryMock<Offer> offersMock = new GenericRepositoryMock<Offer>();

        public bool ChangesSaved { get; set; }

        public IRepository<User> Users
        {
            get { return this.usersMock; }
        }

        public IRepository<Offer> Offers { get; private set; }
        public IRepository<Bid> Bids { get; private set; }
        public IUserStore<User> UserStore { get; private set; }

        public IRepository<Bid> Bugs
        {
            get { return this.bidsMock; }
        }

        public IRepository<Offer> Comments
        {
            get { return this.offersMock; }
        }

        public void SaveChanges()
        {
            this.ChangesSaved = true;
        }
    }
}
