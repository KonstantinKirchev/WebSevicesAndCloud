using System.Linq;
namespace Messages.Data.Repositories
{
    public interface IRepository<T>
    {
        IQueryable<T> All();

        T Find(object id);

        T Add(T entity);

        T Update(T entity);

        void Delete(T entity);

        void Delete(object id);
    }
}
