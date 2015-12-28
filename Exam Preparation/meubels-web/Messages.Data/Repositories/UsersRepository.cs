using Messages.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Messages.Data.Repositories
{
    public class UsersRepository : GenericReposory<User>, IRepository<User>
    {
        public UsersRepository(DbContext context) : base(context)
        {

        }

        public User GetUserById(string id)
        {
            return this.Find(id);
        }
    }
}
