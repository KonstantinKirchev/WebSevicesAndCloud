namespace Battleships.Data
{
    using Battleships.Data.Repositories;
    using Battleships.Models;
    
    public interface IBattleshipsData
    {
        IRepository<ApplicationUser> Users { get; }

        GamesRepository Games { get; } // когато кажа Games ще връщам GamesRepository

        IRepository<Ship> Ships { get; }

        int SaveChanges(); // за да може през цялата data да извикваме SaveChanges
    }
}
