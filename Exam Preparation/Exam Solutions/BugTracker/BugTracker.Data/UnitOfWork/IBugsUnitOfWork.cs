using BugTracker.Data.Models;
using BugTracker.Data.Repositories;

namespace BugTracker.Data.UnitOfWork
{
    public interface IBugsUnitOfWork
    {
        IRepository<User> Users { get; }

        IRepository<Bug> Bugs { get; }

        IRepository<Comment> Comments { get; }

        int SaveChanges();
    }
}
