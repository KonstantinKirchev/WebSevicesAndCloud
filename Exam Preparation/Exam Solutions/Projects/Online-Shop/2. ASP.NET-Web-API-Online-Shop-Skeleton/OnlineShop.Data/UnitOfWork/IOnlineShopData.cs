using OnlineShop.Data.Repositories;
using OnlineShop.Models;

namespace OnlineShop.Data.UnitOfWork
{
    public interface IOnlineShopData
    {
        IRepository<Ad> Ads { get; }

        IRepository<AdType> AdTypes { get; }

        IRepository<Category> Categories { get; }

        IRepository<ApplicationUser> ApplicationUsers { get; }

        int SaveChanges();
    }
}
