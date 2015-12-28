namespace Messages.Data
{
    using System.Data.Entity;

    using Microsoft.AspNet.Identity.EntityFramework;
    using Messages.Models;

    public class MessagesDbContext : IdentityDbContext<User>
    {
        public MessagesDbContext()
            : base("DefaultConnection")
        {
        }
        
        public static MessagesDbContext Create()
        {
            return new MessagesDbContext();
        }

        public IDbSet<Channel> Channels { get; set; }

        public IDbSet<ChannelMessage> ChannelMessages { get; set; }

        public IDbSet<UserMessage> UserMessages { get; set; }
    }
}
