namespace Battleships.Data
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;

    using Battleships.Data.Repositories;
    using Battleships.Models;

    public class BattleshipsData : IBattleshipsData // BattleshipData е data която обгръща всички repositories
    {
        private DbContext context; // тази data трябва да има context
        private IDictionary<Type, object> repositories;  // това го ползваме единствено за lazy loading. Ако е вече създадено дадено repository да не го създаваме отново. Пъхаме repositories в dictionary.

        public BattleshipsData(DbContext context) // конструкорът ще приема отвън context
        {
            this.context = context;
            this.repositories = new Dictionary<Type, object>();
        }

        public IRepository<ApplicationUser> Users
        {
            get { return this.GetRepository<ApplicationUser>(); }
        }

        public GamesRepository Games
        {
            get { return (GamesRepository)this.GetRepository<Game>(); }
        }

        public IRepository<Ship> Ships
        {
            get { return this.GetRepository<Ship>(); }
        }

        public int SaveChanges()
        {
            return this.context.SaveChanges();
        }

        private IRepository<T> GetRepository<T>() where T : class
        {
            var type = typeof(T); // по подаден модел, взима типа на модела
            if (!this.repositories.ContainsKey(type)) // проверяваме дали го има в dictionary с repositories
            {
                var typeOfRepository = typeof(GenericRepository<T>);
                if (type.IsAssignableFrom(typeof(Game)))
                {
                    typeOfRepository = typeof(GamesRepository);
                }

                var repository = Activator.CreateInstance(typeOfRepository, this.context);
                this.repositories.Add(type, repository);
            }

            return (IRepository<T>)this.repositories[type]; // след това се връщам в Battleships.WebServices в папка controllers и изтривам ValuesController и си създавам GamesController
        }
    }
}
