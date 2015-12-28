using Messages.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Messages.Data.Repositories
{
    public class UserMessagesRepository : GenericReposory<UserMessage>
    {
        public UserMessagesRepository(DbContext context) : base(context)
        {
        }

        public IQueryable<UserMessage> GetUserMessages(string userId)
        {
            return this.All()
                .Where(x => x.SendToUserId == userId);
        }
    }
}
