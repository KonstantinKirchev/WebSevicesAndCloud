using System.Linq;

namespace Battleships.Data.Repositories
{
    using System.Data.Entity;

    public class GenericRepository<T> : IRepository<T> where T : class
    {
        private DbContext context;
        private IDbSet<T> set; // Това е нашата колекция от данни.

        public GenericRepository(DbContext context)
        {
            this.context = context;
            this.set = context.Set<T>(); // а тук си взимам данните от context.Set<T> и ги запазвам в set

        }

        public IDbSet<T> Set // това пропърти го правя за да мога да достъпя set в GamesRepository
        {
            get { return this.set; }
        }

        public IQueryable<T> All() // Това ми връща заявка, която може да бъде допълвана и няма да ми тегли данните. Чак когато му кажа ToList ще се материализира заявката.
        {
            return this.set;
        }

        public T Find(object id)
        {
            return this.set.Find(id);
        }

        public void Add(T entity)
        {
            this.ChangeState(entity, EntityState.Added);
        }

        public void Update(T entity)
        {
            this.ChangeState(entity, EntityState.Modified);
        }

        public void Delete(T entity)
        {
            this.ChangeState(entity, EntityState.Deleted);
        }

        public T Delete(object id)
        {
            var entity = this.Find(id);
            this.Delete(entity);
            return entity;
        }

        public int SaveChanges()
        {
            return this.context.SaveChanges();
        }

        private void ChangeState(T entity, EntityState state)
        {
            var entry = this.context.Entry(entity);
            if (entry.State == EntityState.Detached) // тук проверявам дали обекта е вързан към контекста
            {
                this.set.Attach(entity);
            }

            entry.State = state;
        }
    }
}
